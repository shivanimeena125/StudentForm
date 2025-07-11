using Formio.Areas.Admin.Repository;
using Formio.Areas.Admin.Services;
using Formio.Areas.Identity.Data;
using Formio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Formio.Areas.Admin.Controllers;
[Area("Admin")]
public class SubmissionController : Controller
{
    private readonly ISubmissionService _submissionService;
    private readonly UserManager<ApplicationUser> _userManager;

    public SubmissionController(ISubmissionService submissionService, UserManager<ApplicationUser> userManager)
    {
        _submissionService = submissionService;
        _userManager = userManager;

    }
    [HttpPost]
    [Route("api/submit-form")]
    public async Task<IActionResult> SubmitForm([FromBody] FormPreviewModel dto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        await _submissionService.SubmitFormAsync(dto.SubmissionData, dto.formGroupId, user.Id);
        return Ok(new { success = true, message = "Form submitted successfully" });
    }

    [HttpGet]
    public async Task<IActionResult> ViewAllSubmissions()
    {
        var submissions = await _submissionService.GetAllSubmissionsAsync();
        return View(submissions);

    }
    [HttpGet]
    [Route("Admin/Submission/GetFormSchema/{formGroupId}")]
    public async Task<IActionResult> GetFormSchema(Guid formGroupId)
    {
        var formJson = await _submissionService.GetFormSchemaAsync(formGroupId);
        if (formJson == null)
            return NotFound();

        return Content(formJson, "application/json");
    }
    public async Task<IActionResult> ViewSubmission(Guid formGroupId, string submissionData)
    {
        string decodedData = Uri.UnescapeDataString(submissionData);
        var model = await _submissionService.GetFormPreviewDataAsync(formGroupId, decodedData);

        if (model == null)
            return NotFound();

        return View("~/Areas/Admin/Views/Submission/ViewSubmission.cshtml", model);
    }
}

