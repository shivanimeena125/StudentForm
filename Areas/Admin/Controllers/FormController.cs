using Formio.Areas.Admin.Servises;
using Formio.Areas.Identity.Data;
using Formio.Models;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace Formio.Areas.Admin.Controllers
{
    public class FormController : Controller
    {
        private readonly IFormServices _formServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public FormController(UserManager<ApplicationUser> userManager, IFormServices formServices)
        {
            _userManager = userManager;
            _formServices = formServices;
        }
        public async Task<IActionResult> Builder(int id)
        {
         var form = await _formServices.GetFormById(id);
            return View("~/Areas/Admin/views/Form/Builder.cshtml",form);
        }
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> Builder(Forms model)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return Json(new
                {
                    success = false,
                    message = "User is not logged in.",
                    redirectUrl = Url.Action("Identity/Account/login")
                });
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {

                return Json(new { success = false, message = "User not found." });
            }
            model.CreatedBy = user.Id;
            model.Latest = true;
            model.CreatedUtc = DateTime.UtcNow;
            model.VersionId = Guid.NewGuid();

            ModelState.Remove(nameof(model.CreatedBy));

            //foreach (var modelState in ViewData.ModelState)
            //{
            //    var key = modelState.Key;
            //    var errors = modelState.Value.Errors;
            //    foreach (var error in errors)
            //    {
            //        Console.WriteLine($"Property: {key}, Error: {error.ErrorMessage}");
            //    }
            //}

            if (ModelState.IsValid)
            {

                _formServices.AddForm(model);


                return Json(new { success = true, message = "Form created successfully." });
            }


            return Json(new { success = false, message = "Validation failed" });
        }


        [HttpGet]
        public async Task<IActionResult> ViewAllForms()
        {
            var forms = await _formServices.AllForms();
            return View("~/Areas/Admin/Views/Form/ViewAllForms.cshtml", forms);



        }
        public async Task<IActionResult> ViewForm(int id)
        {
            var form = await _formServices.GetFormById(id);
            if (form == null)
            {
                return NotFound();
            }
            return View("~/Areas/Admin/Views/Form/ViewForm.cshtml", form);

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<JsonResult> FormDelete(int id)
        {
            int result = await _formServices.DeleteForm(id);

            if (result > 0)
            {
                return Json(new { success = true, message = "Form deleted successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Form not found." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditForm(int id)
        {
            var form = await _formServices.GetFormById(id);
            if (form == null)
            {
                return NotFound();
            }
            return View("~/Areas/Admin/Views/Form/Builder.cshtml", form);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> EditForm(Forms model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "User is not logged in." });
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            var existingForm = await _formServices.GetFormById(model.Id);
            if (existingForm == null)
            {
                return Json(new { success = false, message = "Form not found." });
            }
            existingForm.Latest= false;
            await _formServices.UpdateForm(existingForm);

            model.CreatedBy = existingForm.CreatedBy; 
            model.VersionId = Guid.NewGuid();
            model.CreatedUtc = existingForm.CreatedUtc; 
            model.FormGroupId = existingForm.FormGroupId;
            model.ModifiedBy = user.Id;
            model.ModifiedUtc = DateTime.UtcNow;
            ModelState.Remove(nameof(model.ModifiedBy));
            if (ModelState.IsValid)
            {
                await _formServices.AddForm(model);
                return Json(new { success = true, message = "Form updated successfully." });
            }
            return Json(new { success = false, message = "Validation failed" });

        }
    }
    }
