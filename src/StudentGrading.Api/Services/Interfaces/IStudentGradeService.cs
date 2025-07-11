using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentGrading.Api.Models;

namespace StudentGrading.Api.Services.Interfaces
{
    public interface IStudentGradeService
    {
        Task<StudentDetailsDto> GetStudentDetailsAsync(int studentId);
        Task<IEnumerable<StudentDetailsDto>> GetAllStudentsWithDetailsAsync();
    }
}