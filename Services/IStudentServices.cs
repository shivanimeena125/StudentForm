using Microsoft.AspNetCore.Mvc;
using StudentForm.Models;

namespace StudentForm.Services
{
    public interface IStudentServices
    {
        Task AddStudent(Student student);

        Task<List<Student>> AllStudent();

        Task<Student> GetStudentById(int id);

        Task UpdateStudent(Student student);

        Task<bool> DeleteStudent(int id);

        List<Student> GetFiltered(string firstName, string lastName, string gender);

        //PaginatedResult<Student> GetFiltered(string firstName, string lastName, string gender, int page = 1, int pageSize = 5);
    }
}
