namespace StudentGrading.Api.Models
{
    public class StudentGrade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public double Percentage { get; set; }

        public string LetterGrade { get; set; } = string.Empty;
        public bool IsPassing { get; set; }

        public Student? Student { get; set; }
        public Lesson? Lesson { get; set; }
    }
}