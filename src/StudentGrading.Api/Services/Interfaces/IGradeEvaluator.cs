namespace StudentGrading.Api.Services.Interfaces
{
    public interface IGradeEvaluator
    {
        (string letterGrade, bool isPassing) EvaluateGrade(double percentage);
    }
}