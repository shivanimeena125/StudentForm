using Formio.Areas.Admin.Services;
using Formio.Areas.Identity.Data;
using Formio.Models;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



namespace Formio.Areas.Admin.Controllers
{
    
    public class FormController : Controller
    {
        private readonly IFormService _formServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public FormController(UserManager<ApplicationUser> userManager, IFormService formServices)
        {
            _userManager = userManager;
            _formServices = formServices;
        }


        [Authorize(Roles ="Admin")]
       [HttpGet]
        public async Task<IActionResult> AddForms()
        {
          
            var model =new Forms();
         
           return View("~/Areas/Admin/views/Form/AddForms.cshtml", model);
        }




       
        [HttpPost]
        public async Task<JsonResult> AddForms(Forms model, ClaimsPrincipal userPrincipal)
        {
            return await _formServices.AddFormAsync(model, User);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewAllForms()
        {
            var forms = await _formServices.ViewFormsAsync();
            return View("~/Areas/Admin/Views/Form/ViewAllForms.cshtml", forms);
        }




        public async Task<IActionResult> ViewForm(Guid formGroupId)
        {
           
            var form = await _formServices.GetFormByFormGroupId(formGroupId);

            if (form == null)
                return NotFound();

            return View("~/Areas/Admin/Views/Form/ViewForm.cshtml", form);

        }




        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<JsonResult> DeleteForm(int id)
        {
            if (id == null)
            {
                return Json(new { success = true, message = "Form not found." });
            }

            return await _formServices.DeleteFormAsync(id);

           
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateForm(Guid formGroupId)
        {
            var form = await _formServices.GetFormByFormGroupId(formGroupId);
            if (form == null)
            {
                return NotFound();
            }
            return View("~/Areas/Admin/Views/Form/AddForms.cshtml", form);
        }



        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<JsonResult> UpdateForm(Forms model)
        {
            ModelState.Remove(nameof(model.CreatedBy));
           
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Validation failed." });
            }

            return await _formServices.UpdateFormAsync(model, User);

           
        }




       
    }
}
