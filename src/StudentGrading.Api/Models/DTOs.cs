namespace StudentGrading.Api.Models
{
    public class StudentGradeDto
    {
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LetterGrade { get; set; } = string.Empty;
        public bool IsPassing { get; set; }
    }

    public class StudentDetailsDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<StudentGradeDto> Grades { get; set; } = new();
        public bool HasPassedYear { get; set; }
    }
}