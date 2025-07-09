using Formio.Areas.Admin.Helpers;
using Formio.Areas.Admin.Repository;
using Formio.Areas.Identity.Data;
using Formio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace Formio.Areas.Admin.Services
{
    public class FormService : IFormService
    {
        private readonly IFromRepository _formRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public FormService(UserManager<ApplicationUser> userManager, IFromRepository formRepository)
        {
            _userManager = userManager;
            _formRepository = formRepository;
        }

        public async Task<bool> AddFormAsync(Forms form, string userId)
        {
            form.CreatedBy = userId;
            form.ModifiedBy = userId;
            form.CreatedUtc = DateTime.UtcNow;
            form.Latest = true;
            form.VersionId = GuidHelper.NewGuid();
            form.FormGroupId = GuidHelper.NewGuid(); 
            await _formRepository.AddFormAsync(form);
            return true;
        }

        public async Task<List<ViewFormModel>> ViewFormsAsync()
        {
            return await _formRepository.ViewFormsAsync();
        }

        public async Task<JsonResult> DeleteFormAsync(int id)
        {
            
            var form = await _formRepository.GetFormByIdAsync(id);
            if (form == null)
            {
                return new JsonResult(new { success = false, message = "Form not found." });
            }

            var row = await _formRepository.DeleteFormAsync(id);
            return row > 0 
                ? new JsonResult(new { success = true, message = "Form delete successfully" })
                : new JsonResult(new { success = false, message = "Form delete failed" });
        }

        public async Task<JsonResult> UpdateFormAsync(Forms form, ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user == null)
                return new JsonResult(new { success = false, message = "User not found." });

            var existingForm = await _formRepository.GetFormByFormGroupId(form.FormGroupId);
            if (existingForm == null)
                return new JsonResult(new { success = false, message = "Form not found." });

            existingForm.Latest = false;
            await _formRepository.UpdateFormAsync(existingForm);

            //form.CreatedBy = existingForm.CreatedBy;
            ////form.VersionId = Guid.NewGuid();
            //form.VersionId=GuidHelper.NewGuid();
            //form.CreatedUtc = existingForm.CreatedUtc;
            //form.FormGroupId = existingForm.FormGroupId;
            //form.ModifiedBy = user.Id;
            //form.Latest = true;
            //form.ModifiedUtc = DateTime.UtcNow;

            FormHelper.FillNewVersionFields(form, existingForm, user.Id);

            await _formRepository.AddFormAsync(form);

            return new JsonResult(new
            {
                success = true,
                message = "Update successfully",
                redirectUrl = "/Admin/Form/ViewAllForms"
            });
        }
        public async Task<Forms> GetFormByFormGroupId(Guid formGroupId)
        {
            if (formGroupId == Guid.Empty)
                throw new ArgumentException("Invalid FormGroupId");

            return await _formRepository.GetFormByFormGroupId(formGroupId);
        }
        public async Task<object> GetPaginatedFormsAsync(DataTableRequest request, string title, string startDate, string endDate)
        {
            var (data, total, filtered) = await _formRepository.GetPaginatedFormsAsync(
                title,
                startDate,
                endDate,
                request.Start,
                request.Length
            );

            return new
            {
                draw = request.Draw,
                recordsTotal = total,           
                recordsFiltered = filtered,     
                data = data
            };
        }


    }
}