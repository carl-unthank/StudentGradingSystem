using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentGrading.Api.Repositories.Interfaces;
using StudentGrading.Api.Models;

namespace StudentGrading.Api.Repositories.Implementations
{
    public class InMemoryStudentRepository : IStudentRepository
    {
        private static readonly List<Student> _students = new()
        {
            new Student
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                StudentGrades = new List<StudentGrade>
                {
                    new StudentGrade { Id = 1, StudentId = 1, LessonId = 1, Percentage = 85.5f },
                    new StudentGrade { Id = 2, StudentId = 1, LessonId = 2, Percentage = 92f },
                    new StudentGrade { Id = 3, StudentId = 1, LessonId = 3, Percentage = 78.5f }
                }
            },
            new Student
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                StudentGrades = new List<StudentGrade>
                {
                    new StudentGrade { Id = 4, StudentId = 2, LessonId = 1, Percentage = 95f },
                    new StudentGrade { Id = 5, StudentId = 2, LessonId = 2, Percentage = 88f },
                    new StudentGrade { Id = 6, StudentId = 2, LessonId = 3, Percentage = 68.5f },
                    new StudentGrade { Id = 7, StudentId = 2, LessonId = 4, Percentage = 75f },
                    new StudentGrade { Id = 8, StudentId = 2, LessonId = 5, Percentage = 80f }
                }
            },
            new Student { Id = 3, FirstName = "Bob", LastName = "Johnson" },
            new Student
            {
                Id = 4,
                FirstName = "Alice",
                LastName = "Williams",
                StudentGrades = new()
                {
                    new StudentGrade { Id = 9, StudentId = 4, LessonId = 1, Percentage = 70f },
                    new StudentGrade { Id = 10, StudentId = 4, LessonId = 2, Percentage = 65f },
                    new StudentGrade { Id = 11, StudentId = 4, LessonId = 3, Percentage = 40f },
                    new StudentGrade { Id = 12, StudentId = 4, LessonId = 4, Percentage = 50f },
                    new StudentGrade { Id = 13, StudentId = 4, LessonId = 5, Percentage = 55f }
                }
             },
            new Student
            {
                Id = 5,
                FirstName = "Charlie",
                LastName = "Brown",
                StudentGrades = new List<StudentGrade>
                {
                    new StudentGrade { Id = 14, StudentId = 5, LessonId = 1, Percentage = 82f },
                    new StudentGrade { Id = 15, StudentId = 5, LessonId = 2, Percentage = 78f },
                    new StudentGrade { Id = 16, StudentId = 5, LessonId = 3, Percentage = 85f },
                    new StudentGrade { Id = 17, StudentId = 5, LessonId = 4, Percentage = 79f },
                    new StudentGrade { Id = 18, StudentId = 5, LessonId = 5, Percentage = 88f }
                }
            }
        };

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            var copiedStudents = _students.Select(s => new Student
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                StudentGrades = s.StudentGrades.Select(g => new StudentGrade
                {
                    Id = g.Id,
                    StudentId = g.StudentId,
                    LessonId = g.LessonId,
                    Percentage = g.Percentage
                }).ToList()
            }).ToList();

            return await Task.FromResult<IEnumerable<Student>>(copiedStudents);
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            var student = _students.FirstOrDefault(s => s.Id == studentId) ?? throw new ArgumentException($"Student with ID {studentId} not found.");
            
            return await Task.FromResult(new Student
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                StudentGrades = student.StudentGrades.Select(sg => new StudentGrade
                {
                    Id = sg.Id,
                    StudentId = sg.StudentId,
                    LessonId = sg.LessonId,
                    Percentage = sg.Percentage
                }).ToList()
            });
        }
    }
}