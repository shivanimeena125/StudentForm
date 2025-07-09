using Formio.Areas.Admin.Services;
using Formio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Formio.Areas.Admin.Controllers;
[Area("Admin")]
public class SubmissionController : Controller
{
    private readonly ISubmissionService _submissionService;

    public SubmissionController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }
    
    [HttpPost]
    public async Task<IActionResult> FormSubmissionAsync(int FormId, string SubmissionData)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var submission = new FormSubmission
        {
            FormId = FormId,
            SubmissionData = SubmissionData,
        };
        var isAdded = await _submissionService.FormSubmissionAsync(submission, userId);

        return Ok(new
        {
            success = isAdded,
            message = isAdded ? "Form submitted successfully." : "Failed to submit form."
        });
    }
    }

