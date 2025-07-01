using Formio.Models;
using Formio.Repository;
using Formio.Servises;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Formio.Controllers
{
    public class FormController : Controller
    {
        private readonly IFormServices _formServices;
        private readonly UserManager<IdentityUser> _userManager;

        public FormController(UserManager<IdentityUser> userManager, IFormServices formServices)
        {
            _userManager = userManager;
            _formServices = formServices;
        }
        public IActionResult Builder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> saveForm(Forms model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            model.CreatedBy = user.Id ;

            model.CreatedUtc = DateTime.UtcNow;
            model.VersionId = Guid.NewGuid();
            model.Latest = true;

            _formServices.AddForm(model);

            return RedirectToAction("Builder");
        }



    }
}
