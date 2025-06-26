using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentForm.Models;
using StudentForm.Services;
using System.Threading.Tasks;

namespace StudentForm.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentServices _studentService;

        public StudentController(IStudentServices studentServices)
        {
            _studentService = studentServices;
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

            var viewModel = new ViewStudentModel
            {
                Student = student,
                AllStudents = students
            };

            return View(viewModel);
           
        }

        [HttpPost]
        public async Task<JsonResult> AddStudent(Student student)
        {
            if (ModelState.IsValid)
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

            }
            ModelState.Clear();
            return Json(new { success = false, message = "something wrong..." });
        }




        [HttpGet]
        public async Task<JsonResult> AllStudent(string firstName, string lastName, string gender)
        {
            var students = _studentService.GetFiltered(firstName, lastName, gender);
            return Json(students);
        }


        //[HttpGet]
        //public JsonResult AllStudent(string firstName, string lastName, string gender, int page = 1)
        //{
        //    var result = _studentService.GetFiltered(firstName, lastName, gender, page, 10);
        //    return Json(result);
        //}



        [HttpGet]
        public async Task<JsonResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentById(id);
            if (student == null)
                return Json(null);

            return Json(student);
        }



        [HttpGet]
        public async Task<IActionResult> EditStudent(int id)
        {
            var student = await _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }


        [HttpPost]
        public async Task<JsonResult> EditStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentService.UpdateStudent(student);

                return Json(new { success = true, message = "Student updated successfully!" });
            }

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
