using Formio.Areas.Admin.Servises;
using Formio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Formio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public readonly IFormServices _formService;

        public HomeController(ILogger<HomeController> logger,IFormServices formServices)
        {
            _logger = logger;
            _formService = formServices;
        }

        public async Task<IActionResult> Index()
        {
           
            return View();
        }

        public async Task<IActionResult> OnlineForms()
        {
            var forms = await _formService.AllForms();
            return View(forms);
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
