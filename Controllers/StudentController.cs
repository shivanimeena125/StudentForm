using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentForm.Data;
using StudentForm.Models;
using StudentForm.Services;
using System.Threading.Tasks;

namespace StudentForm.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentServices _studentService;

        public readonly ICity _city;

        public StudentController(IStudentServices studentServices,ICity city)
        {
            _studentService = studentServices;
           _city = city;
        }


        [HttpGet]
        public async Task<IActionResult> AddStudent(int? id)
        {
            Student student = new Student();

            if (id != null)
            {
                student = await _studentService.GetStudentById(id.Value);
            }
            ModelState.Clear();

            var students = await _studentService.AllStudent();

            var cities = await _city.GetAllCities();
            var cityList = cities.Select(c => new SelectListItem
            {
                Text = c.CityName,
                Value = c.Id.ToString()
            }).ToList();

            var viewModel = new ViewStudentModel
            {
                Student = student,
                AllStudents = students,
                CityList = cityList
            };

            return View(viewModel);

        }

        [HttpPost]
        public async Task<JsonResult> AddStudent(Student student)
        {
           
                if (student.Id == 0)
                {
                    await _studentService.AddStudent(student);
                    return Json(new { success = true, message = " add successfully." });
                }
                else
                {
                    await _studentService.UpdateStudent(student);
                    return Json(new { success = true, message = "Update successfully." });
                }

           
            ModelState.Clear();
            return Json(new { success = false, message = "something wrong..." });
        }




        [HttpGet]
        public async Task<JsonResult> AllStudent(string firstName, string lastName, string gender, int cityId, int stateId, int countryId)
        {
            var students = _studentService.GetFiltered(firstName, lastName, gender,cityId,stateId,countryId);
            var result = students.Select(s => new {
                s.Id,
                s.FirstName,
                s.LastName,
                s.Gender,
                s.Class,
                s.Address,
               
                CityName = s.City?.CityName,
             
                StateName = s.City?.State?.StateName,
             
                CountryName = s.City?.State?.Country?.CountryName
            });
            return Json(result);
        }


        //[HttpGet]
        //public JsonResult AllStudent(string firstName, string lastName, string gender, int page = 1)
        //{
        //    var result = _studentService.GetFiltered(firstName, lastName, gender, page, 5);
        //    return Json(result);
        //}



        [HttpGet]
        public async Task<JsonResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentById(id);

            if (student == null)
                return Json(null);

            //return Json(student);

            return Json(new
            {
                id = student.Id,
                firstName = student.FirstName,
                lastName = student.LastName,
                gender = student.Gender,
                @class = student.Class,
                address = student.Address,

                cityId = student.City?.Id,
                cityName = student.City?.CityName,

                stateId = student.City?.State?.Id,
                stateName = student.City?.State?.StateName,

                countryId = student.City?.State?.Country?.Id,
                countryName = student.City?.State?.Country?.CountryName
            });
        }



        [HttpGet]
        public async Task<IActionResult> EditStudent(int id)
        {
            var student = await _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            var cities = await _city.GetAllCities();
            var cityList = cities.Select(c => new SelectListItem
            {
                Text = c.CityName,
                Value = c.Id.ToString()
            }).ToList();

            var viewModel = new ViewStudentModel
            {
                Student = student,
                CityList = cityList
            };

            return View(viewModel);

        }


        [HttpPost]
        public async Task<JsonResult> EditStudent(Student student)
        {
                await _studentService.UpdateStudent(student);

                return Json(new { success = true, message = "Student updated successfully!" });
            

            return Json(new { success = false, message = "Validation failed!" });
        }



        [HttpPost]
        public async Task<JsonResult> DeleteStudent(int id)
        {
            
            bool Deleted = await _studentService.DeleteStudent(id);

            if (Deleted)
            {
                return Json(new { success = true, message = "User deleted successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Student not found!" });
            }
        }


      

    }
}
