using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentForm.Models
{
    public class ViewStudentModel
    {
        public Student Student { get; set; }
        public List<Student> AllStudents { get; set; }

        public List<SelectListItem> CityList { get; set; }
    }
}
