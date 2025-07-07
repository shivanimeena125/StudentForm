using Formio.Areas.Admin.Repository;
using Formio.Areas.Identity.Data;
using Formio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Formio.Areas.Admin.Services { 
public class FormService: IFormService
    {
    private readonly FormRepository _formRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public FormService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _formRepository = new FormRepository(configuration); 
    }

    public async Task<JsonResult> AddFormAsync(Forms form, ClaimsPrincipal userPrincipal)
    {
            if (!userPrincipal.Identity.IsAuthenticated)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "User is not logged in.",
                    redirectUrl = "/Identity/Account/Login"
                });
            }

            var user = await _userManager.GetUserAsync(userPrincipal);
        if (user == null)
        {
            return new JsonResult(new { success = false, message = "User not found." ,});
        }

        form.CreatedBy = user.Id;
        form.CreatedUtc = DateTime.UtcNow;
        form.Latest = true;
        form.VersionId = Guid.NewGuid();
        form.FormGroupId = Guid.NewGuid();

        await _formRepository.AddFormAsync(form);

        return new JsonResult(new { success = true, message = "Form created successfully.",
            redirectUrl = "/Admin/Form/ViewAllForms"
        });
    }


        public async Task<List<ViewFormModel>> ViewFormsAsync()
        {
            return await _formRepository.ViewFormsAsync();
        }

       



        public async Task<JsonResult> DeleteFormAsync(int id)
        {
            if (id <= 0)
            {
                return new JsonResult(new { success = false, message = "Ivalid Form id" });
            }
            var form = await _formRepository.GetFormByIdAsync(id);
            if (form == null)
            {
                return new JsonResult(new { success = false, message = "Form not found." });
            }
            var row= await _formRepository.DeleteFormAsync(id);

            if (row > 0) {
                return new JsonResult(new { success = true, message = "Form delete successfully" });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Form delete failed" });
            }
        }



        public async Task<JsonResult> UpdateFormAsync(Forms form, ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user == null)
                return new JsonResult(new { success = false, message = "User not found." });

            var existingForm = await _formRepository.GetFormByIdAsync(form.Id);
            if (existingForm == null)
                return new JsonResult(new { success = false, message = "Form not found." });


            existingForm.Latest = false;
            await _formRepository.UpdateFormAsync(existingForm);

            
            form.CreatedBy = existingForm.CreatedBy;
            form.VersionId = Guid.NewGuid();
            form.CreatedUtc = existingForm.CreatedUtc;
            form.FormGroupId = existingForm.FormGroupId;
            form.ModifiedBy = user.Id;
            form.Latest = true;
            form.ModifiedUtc = DateTime.UtcNow;



            await _formRepository.AddFormAsync(form);

            return new JsonResult(new { success = true, message = "Update successfully",
                redirectUrl = "/Admin/Form/ViewAllForms"
            });
        }



        public async Task<Forms> GetFormByFormGroupId(Guid formGroupId)
        {
            if (formGroupId == Guid.Empty)
                throw new ArgumentException("Invalid FormGroupId");

            return await _formRepository.GetFormByFormGroupId(formGroupId);
        }



    }

}