using Formio.Areas.Admin.Services;
using Formio.Areas.Identity.Data;
using Formio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Formio.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
public class FormController : Controller
{
    private readonly IFormService _formServices;
    private readonly UserManager<ApplicationUser> _userManager;

    public FormController(UserManager<ApplicationUser> userManager, IFormService formServices)
    {
        _userManager = userManager;
        _formServices = formServices;
    }

    [HttpGet]
    public IActionResult AddForms()
    {
        var model = new Forms();
        return View("~/Areas/Admin/views/Form/AddForms.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> AddForms(Forms model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var isAdded = await _formServices.AddFormAsync(model, userId);
        return Ok(new
        {
            success = isAdded,
            message = isAdded ? "Form added successfully." : "Failed to add form.",
            redirectUrl = "/Admin/Form/ViewAllForms"
        });
    }

    [HttpGet]
    public async Task<IActionResult> ViewAllForms()
    {
        var forms = await _formServices.ViewFormsAsync();
        return View("~/Areas/Admin/Views/Form/ViewAllForms.cshtml", forms);
    }

    [HttpGet("/Form/ViewForm/{formGroupId}")]
    public async Task<IActionResult> ViewForm(Guid formGroupId)
    {
        var form = await _formServices.GetFormByFormGroupId(formGroupId);
        if (form == null)
            return NotFound();

        return View("~/Areas/Admin/Views/Form/ViewForm.cshtml", form);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteForm(int id)
    {
        return await _formServices.DeleteFormAsync(id);
    }

    [HttpGet("/Form/UpdateForm/{formGroupId}")]
    public async Task<IActionResult> UpdateForm(Guid formGroupId)
    {
        var form = await _formServices.GetFormByFormGroupId(formGroupId);
        if (form == null)
        {
            return NotFound();
        }

        return View("~/Areas/Admin/Views/Form/AddForms.cshtml", form);
    }


    public async Task<JsonResult> UpdateForm(Forms model)
    {
        ModelState.Remove(nameof(model.CreatedBy));
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Validation failed." });
        }

        return await _formServices.UpdateFormAsync(model, User);
    }
    [HttpPost]
    public async Task<IActionResult> LoadFormData([FromForm] DataTableRequest request)
    {

        var title = Request.Form["title"];
        var startDate = Request.Form["startDate"];
        var endDate = Request.Form["endDate"];

        var result = await _formServices.GetPaginatedFormsAsync(request, title, startDate, endDate);
        return Json(result);
    }




}