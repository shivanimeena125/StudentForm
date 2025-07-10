using Formio.Areas.Admin.Repository;
using Formio.Areas.Admin.Services;
using Formio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Formio.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public readonly IFormService _formService;
        public readonly ISubmissionService _submissionService;
        public readonly IFromRepository _formRepository;

        public HomeController(ILogger<HomeController> logger, IFormService formServices,ISubmissionService submissionService, IFromRepository fromRepository)
        {
            _logger = logger;
            _formService = formServices;
            _formRepository = fromRepository;
            _submissionService = submissionService;

        }

        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> OnlineForms()
        {
            var forms = await _formService.ViewFormsAsync();
            return View(forms);
        }
        public async Task<IActionResult> ViewForm(Guid formGroupId)
        {
            var form = await _formService.GetFormByFormGroupId(formGroupId);
            if (form == null)
                return NotFound();

            return View("~/Areas/Admin/Views/Form/ViewForm.cshtml", form);
        }
        [HttpGet]
        public async Task<IActionResult> GetFormSchema(int id)
        {
            var form = await _formRepository.GetFormByIdAsync(id);
            if (form == null) return NotFound();

            return Content(form.FormFields, "application/json");
        }
        [HttpGet]
        public async Task<IActionResult> ViewAllSubmissions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var submissions = await _submissionService.GetSubmissionsByUserAsync(userId);
            return View(submissions);

        }
        public async Task<IActionResult> ViewSubmission(int formId, string submissionData)
        {
            string decodedData = Uri.UnescapeDataString(submissionData);
            var model = await _submissionService.GetFormPreviewDataAsync(formId, decodedData);

            if (model == null)
                return NotFound();

            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}