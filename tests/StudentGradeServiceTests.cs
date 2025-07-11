using Moq;
using StudentGrading.Api.Models;
using StudentGrading.Api.Repositories.Interfaces;
using StudentGrading.Api.Services.Implementations;
using StudentGrading.Api.Services.Interfaces;
using Xunit;

namespace StudentGrading.Tests
{
    public class StudentGradeServiceTests
    {
        private readonly Mock<IStudentRepository> _mockStudentRepository;
        private readonly Mock<ILessonRepository> _mockLessonRepository;
        private readonly Mock<IGradeEvaluator> _mockGradeEvaluator;
        private readonly StudentGradeService _service;

        public StudentGradeServiceTests()
        {
            _mockStudentRepository = new Mock<IStudentRepository>();
            _mockLessonRepository = new Mock<ILessonRepository>();
            _mockGradeEvaluator = new Mock<IGradeEvaluator>();
            _service = new StudentGradeService(_mockStudentRepository.Object, _mockLessonRepository.Object, _mockGradeEvaluator.Object);
        }

        [Fact]
        public async Task GetAllStudentsWithDetailsAsync_ReturnsMultipleStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    StudentGrades = new List<StudentGrade>
                    {
                        new StudentGrade { StudentId = 1, LessonId = 1, Percentage = 92f },
                        new StudentGrade { StudentId = 1, LessonId = 2, Percentage = 88f },
                        new StudentGrade { StudentId = 1, LessonId = 3, Percentage = 85f }
                    }
                }
            };

            var lessons = new List<Lesson>
            {
                new Lesson { Id = 1, Name = "Mathematics" },
                new Lesson { Id = 2, Name = "Science" },
                new Lesson { Id = 3, Name = "English" }
            };

            _mockStudentRepository.Setup(r => r.GetAllStudentsAsync()).ReturnsAsync(students);
            _mockLessonRepository.Setup(r => r.GetAllLessonsAsync()).ReturnsAsync(lessons);
            _mockGradeEvaluator.Setup(e => e.EvaluateGrade(92f)).Returns(("A", true));
            _mockGradeEvaluator.Setup(e => e.EvaluateGrade(88f)).Returns(("B", true));
            _mockGradeEvaluator.Setup(e => e.EvaluateGrade(85f)).Returns(("B", true));

            // Act
            var result = await _service.GetAllStudentsWithDetailsAsync();

            // Assert
            var resultList = result.ToList();
            Assert.Single(resultList);
            Assert.Equal("Alice Johnson", resultList[0].Name);
            Assert.True(resultList[0].HasPassedYear);
            Assert.Equal(3, resultList[0].Grades.Count);
        }

        [Fact]
        public void CheckStudentYearPassStatus_ReturnsTrue_WhenThreePassingGrades()
        {
            // Arrange
            var grades = new List<StudentGradeDto>
            {
                new StudentGradeDto { LessonId = 1, IsPassing = true },
                new StudentGradeDto { LessonId = 2, IsPassing = true },
                new StudentGradeDto { LessonId = 3, IsPassing = true }
            };

            // Act
            var result = _service.CheckStudentYearPassStatus(grades);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckStudentYearPassStatus_ReturnsFalse_WhenLessThanThreePassingGrades()
        {
            // Arrange
            var grades = new List<StudentGradeDto>
            {
                new StudentGradeDto { LessonId = 1, IsPassing = true },
                new StudentGradeDto { LessonId = 2, IsPassing = false }
            };

            // Act
            var result = _service.CheckStudentYearPassStatus(grades);

            // Assert
            Assert.False(result);
        }
    }
}
