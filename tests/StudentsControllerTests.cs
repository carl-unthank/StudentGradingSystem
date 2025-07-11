using Microsoft.AspNetCore.Mvc;
using Moq;
using StudentGrading.Api.Controllers;
using StudentGrading.Api.Models;
using StudentGrading.Api.Services.Interfaces;
using Xunit;

namespace StudentGrading.Tests
{
    public class StudentsControllerTests
    {
        private readonly Mock<IStudentGradeService> _mockStudentService;
        private readonly StudentsController _controller;

        public StudentsControllerTests()
        {
            _mockStudentService = new Mock<IStudentGradeService>();
            _controller = new StudentsController(_mockStudentService.Object);
        }

        [Fact]
        public async Task GetAllStudentsWithDetails_ReturnsOkResult_WithStudentList()
        {
            // Arrange
            var expectedStudents = new List<StudentDetailsDto>
            {
                new StudentDetailsDto
                {
                    Id = 1,
                    Name = "John Doe",
                    HasPassedYear = true,
                    Grades = new List<StudentGradeDto>
                    {
                        new StudentGradeDto
                        {
                            LessonId = 1,
                            LessonName = "Math",
                            LetterGrade = "B",
                            IsPassing = true
                        }
                    }
                },
                new StudentDetailsDto
                {
                    Id = 2,
                    Name = "Jane Smith",
                    HasPassedYear = false,
                    Grades = new List<StudentGradeDto>
                    {
                        new StudentGradeDto
                        {
                            LessonId = 1,
                            LessonName = "Math",
                            LetterGrade = "F",
                            IsPassing = false
                        }
                    }
                }
            };

            _mockStudentService.Setup(s => s.GetAllStudentsWithDetailsAsync())
                .ReturnsAsync(expectedStudents);

            // Act
            var result = await _controller.GetAllStudentsWithDetails();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualStudents = Assert.IsType<List<StudentDetailsDto>>(okResult.Value);
            Assert.Equal(2, actualStudents.Count);
            Assert.Equal("John Doe", actualStudents[0].Name);
            Assert.True(actualStudents[0].HasPassedYear);
            Assert.Equal("Jane Smith", actualStudents[1].Name);
            Assert.False(actualStudents[1].HasPassedYear);
        }

        [Fact]
        public async Task GetAllStudentsWithDetails_ReturnsEmptyList_WhenNoStudents()
        {
            // Arrange
            var expectedStudents = new List<StudentDetailsDto>();
            _mockStudentService.Setup(s => s.GetAllStudentsWithDetailsAsync())
                .ReturnsAsync(expectedStudents);

            // Act
            var result = await _controller.GetAllStudentsWithDetails();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualStudents = Assert.IsType<List<StudentDetailsDto>>(okResult.Value);
            Assert.Empty(actualStudents);
        }

        [Fact]
        public async Task GetStudentDetails_ReturnsOkResult_WithStudentDetails()
        {
            // Arrange
            var studentId = 1;
            var expectedStudent = new StudentDetailsDto
            {
                Id = studentId,
                Name = "John Doe",
                HasPassedYear = true,
                Grades = new List<StudentGradeDto>
                {
                    new StudentGradeDto
                    {
                        LessonId = 1,
                        LessonName = "Math",
                        LetterGrade = "B",
                        IsPassing = true
                    }
                }
            };

            _mockStudentService.Setup(s => s.GetStudentDetailsAsync(studentId))
                .ReturnsAsync(expectedStudent);

            // Act
            var result = await _controller.GetStudentDetails(studentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualStudent = Assert.IsType<StudentDetailsDto>(okResult.Value);
            Assert.Equal(studentId, actualStudent.Id);
            Assert.Equal("John Doe", actualStudent.Name);
            Assert.True(actualStudent.HasPassedYear);
        }

        [Fact]
        public async Task GetStudentDetails_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            var studentId = 999;
            _mockStudentService.Setup(s => s.GetStudentDetailsAsync(studentId))
                .Throws(new ArgumentException($"Student with ID {studentId} not found."));

            // Act
            var result = await _controller.GetStudentDetails(studentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Student with ID {studentId} not found.", notFoundResult.Value);
        }
    }
}
