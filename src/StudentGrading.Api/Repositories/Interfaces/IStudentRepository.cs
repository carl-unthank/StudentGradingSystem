using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentGrading.Api.Models;

namespace StudentGrading.Api.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int studentId);
    }
}