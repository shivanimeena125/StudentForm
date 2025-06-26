using Microsoft.AspNetCore.Mvc;
using StudentForm.Services;

namespace StudentForm.Controllers
{
    public class CityController : Controller
    {
        private readonly ICity _city;
        public CityController(ICity city)
        {
            _city = city;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetAllCountries()
        {
            var countries = await _city.GetAllCountries();
            return Json(countries.Select(c => new { c.Id, c.CountryName }));
        }

        [HttpGet]
        public async Task<JsonResult> GetStatesByCountryId(int countryId)
        {
            var states = await _city.GetStatesByCountryId(countryId);
            return Json(states.Select(s => new { s.Id, s.StateName }));
        }

        [HttpGet]
        public async Task<JsonResult> GetCitiesByStateId(int stateId)
        {
            var cities = await _city.GetCitiesByStateId(stateId);
            return Json(cities.Select(c => new { c.Id, c.CityName }));
        }

    }
}
