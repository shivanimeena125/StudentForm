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
    private readonly IFromRepository _formRepository; 
    private readonly UserManager<ApplicationUser> _userManager;

    public SubmissionController(ISubmissionService submissionService, IFromRepository formRepository,UserManager<ApplicationUser> userManager)
    {
        _submissionService = submissionService;
        _userManager = userManager;
        _formRepository = formRepository; 
    }

    [HttpPost]
    [Route("api/submit-form")]
    public async Task<IActionResult> SubmitForm([FromBody] FormSubmissionDto dto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        await _submissionService.SubmitFormAsync(dto.SubmissionData, dto.FormId, user.Id);
        return Ok(new { success = true, message = "Form submitted successfully" });
    }

    [HttpGet]

    public async Task<IActionResult> ViewAllSubmissions()
    {
        var submissions = await _submissionService.GetAllSubmissionsAsync();
        return View(submissions);

    }
    [HttpGet]
    [Route("Admin/Submission/GetFormSchema/{id}")]
    public async Task<IActionResult> GetFormSchema(int id)
    {
        var form = await _formRepository.GetFormByIdAsync(id);  // Inject repo
        if (form == null)
            return NotFound();

        return Content(form.FormFields, "application/json");
    }



}

public class FormSubmissionDto
{
    public int FormId { get; set; }
    public string SubmissionData { get; set; }
}
    

