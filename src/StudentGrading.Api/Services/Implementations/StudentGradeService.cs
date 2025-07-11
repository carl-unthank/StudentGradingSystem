using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentGrading.Api.Services.Interfaces;
using StudentGrading.Api.Repositories.Interfaces;
using StudentGrading.Api.Models;

namespace StudentGrading.Api.Services.Implementations
{
    public class StudentGradeService : IStudentGradeService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IGradeEvaluator _gradeEvaluator;

        public StudentGradeService(IStudentRepository studentRepository, ILessonRepository lessonRepository, IGradeEvaluator gradeEvaluator)
        {
            _studentRepository = studentRepository;
            _lessonRepository = lessonRepository;
            _gradeEvaluator = gradeEvaluator;
        }

        public async Task<IEnumerable<StudentDetailsDto>> GetAllStudentsWithDetailsAsync()
        {
            var allStudentsFromRepo = await _studentRepository.GetAllStudentsAsync();

            var allLessons = (await _lessonRepository.GetAllLessonsAsync()).ToDictionary(l => l.Id);

            var allStudentDetails = new List<StudentDetailsDto>();

            foreach (var student in allStudentsFromRepo)
            {
                var gradesDto = student.StudentGrades.Select(sg =>
                {
                    var (letterGrade, isPassing) = _gradeEvaluator.EvaluateGrade(sg.Percentage);

                    return new StudentGradeDto
                    {
                        StudentId = sg.StudentId,
                        LessonId = sg.LessonId,
                        LessonName = allLessons.TryGetValue(sg.LessonId, out var lesson) ? lesson.Name : "Unknown Lesson",
                        LetterGrade = letterGrade,
                        IsPassing = isPassing
                    };
                }).ToList();

                var hasPassedYear = CheckStudentYearPassStatus(gradesDto);

                allStudentDetails.Add(new StudentDetailsDto
                {
                    Id = student.Id,
                    Name = $"{student.FirstName} {student.LastName}",
                    Grades = gradesDto,
                    HasPassedYear = hasPassedYear
                });
            }

            return allStudentDetails;
        }

        public async Task<StudentDetailsDto> GetStudentDetailsAsync(int studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            var allLessons = (await _lessonRepository.GetAllLessonsAsync()).ToDictionary(l => l.Id);

            var gradesDto = student.StudentGrades.Select(sg =>
            {
                var (letterGrade, isPassing) = _gradeEvaluator.EvaluateGrade(sg.Percentage);

                return new StudentGradeDto
                {
                    StudentId = sg.StudentId,
                    LessonId = sg.LessonId,
                    LessonName = allLessons.TryGetValue(sg.LessonId, out var lesson) ? lesson.Name : "Unknown Lesson",
                    LetterGrade = letterGrade,
                    IsPassing = isPassing
                };
            }).ToList();

            var hasPassedYear = CheckStudentYearPassStatus(gradesDto);

            return new StudentDetailsDto
            {
                Id = student.Id,
                Name = $"{student.FirstName} {student.LastName}",
                Grades = gradesDto,
                HasPassedYear = hasPassedYear
            };
        }

        public bool CheckStudentYearPassStatus(List<StudentGradeDto> studentGrades)
        {
            var passingGrades = studentGrades.Where(g => g.IsPassing).ToList();

            var distinctPassingLessons = passingGrades
                .Select(g => g.LessonId)
                .Distinct()
                .Count();

            return passingGrades.Count >= 3 && distinctPassingLessons >= 3;
        }
    }
}