using System.Collections.Generic;

namespace StudentGrading.Api.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<StudentGrade> StudentGrades { get; set; } = new();
    }
}