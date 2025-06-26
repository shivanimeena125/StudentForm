using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentForm.Data;
using StudentForm.Models;
using StudentForm.Services;

namespace StudentForm.Repository
{
    public class StudentRepository:IStudentServices
{
        private readonly MyDBContext _context;

        public StudentRepository(MyDBContext context)
        {
            _context = context;
        }

        public async Task AddStudent(Student student)
        {
            _context.StudentForm.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Student>> AllStudent()
        {
            return await _context.StudentForm.ToListAsync();
        }


         public async Task<Student> GetStudentById(int id)
        {
             return await _context.StudentForm.FirstOrDefaultAsync(s => s.Id == id);    
        }

        public async Task UpdateStudent(Student student)
        {
            _context.StudentForm.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var student = await _context.StudentForm.FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return false;
            }
            _context.StudentForm.Remove(student);
            await _context.SaveChangesAsync(); 

            return true;
        }

        public List<Student> GetFiltered(string firstName, string lastName, string gender,string country,string state, string city)
        {
            //var students = _context.StudentForm.AsQueryable();
            var students = _context.StudentForm
        .Include(s => s.City)
        .ThenInclude(c => c.State)
        .ThenInclude(st => st.Country)
        .AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
                students = students.Where(s => s.FirstName.Contains(firstName));

            if (!string.IsNullOrEmpty(lastName))
                students = students.Where(s => s.LastName.Contains(lastName));

            if (!string.IsNullOrEmpty(gender))
                students = students.Where(s => s.Gender == gender);

            if (!string.IsNullOrEmpty(city))
                students = students.Where(s => s.City.CityName == city);

            if (!string.IsNullOrEmpty(state))
                students = students.Where(s => s.City.State.StateName == state);

            if (!string.IsNullOrEmpty(country))
                students = students.Where(s => s.City.State.Country.CountryName == country);

            return students.ToList();
        }


        //public PaginatedResult<Student> GetFiltered(string firstName, string lastName, string gender, int page = 1, int pageSize = 5)
        //{
        //    var students = _context.StudentForm.AsQueryable();

        //    if (!string.IsNullOrEmpty(firstName))
        //        students = students.Where(s => s.FirstName.Contains(firstName));

        //    if (!string.IsNullOrEmpty(lastName))
        //        students = students.Where(s => s.LastName.Contains(lastName));

        //    if (!string.IsNullOrEmpty(gender))
        //        students = students.Where(s => s.Gender == gender);

        //    int totalRecords = students.Count();

        //    var data = students
        //        .OrderBy(s => s.Id)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    return new PaginatedResult<Student>
        //    {
        //        Data = data,
        //        CurrentPage = page,
        //        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
        //        TotalRecords = totalRecords
        //    };
        //}

    }
}
