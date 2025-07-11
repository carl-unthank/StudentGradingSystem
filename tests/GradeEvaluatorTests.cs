using StudentGrading.Api.Models;
using StudentGrading.Api.Services.Implementations;
using StudentGrading.Api.Services.Interfaces;
using Xunit;

namespace StudentGrading.Tests
{
    public class GradeEvaluatorTests
    {
        private readonly IGradeEvaluator _gradeEvaluator;

        public GradeEvaluatorTests()
        {
            var gradeBoundaries = new List<GradeBoundary>
            {
                new GradeBoundary(90f, "A"),
                new GradeBoundary(80f, "B"),
                new GradeBoundary(70f, "C"),
                new GradeBoundary(60f, "D"),
                new GradeBoundary(0f, "F")
            };
            _gradeEvaluator = new GradeEvaluator(gradeBoundaries, 70);
        }

        [Theory]
        [InlineData(95f, "A", true)]
        [InlineData(94f, "A", true)]
        [InlineData(90f, "A", true)]
        [InlineData(89f, "B", true)]
        [InlineData(85f, "B", true)]
        [InlineData(80f, "B", true)]
        [InlineData(79f, "C", true)]
        [InlineData(75f, "C", true)]
        [InlineData(70f, "C", true)]
        [InlineData(69f, "D", false)]
        [InlineData(65f, "D", false)]
        [InlineData(60f, "D", false)]
        [InlineData(59f, "F", false)]
        [InlineData(50f, "F", false)]
        [InlineData(25f, "F", false)]
        [InlineData(0f, "F", false)]
        public void EvaluateGrade_ReturnsCorrectGradeAndPassingStatus_ForVariousScores(float score, string expectedLetterGrade, bool expectedIsPassing)
        {
            // Act
            var result = _gradeEvaluator.EvaluateGrade(score);

            // Assert
            Assert.Equal(expectedLetterGrade, result.letterGrade);
            Assert.Equal(expectedIsPassing, result.isPassing);
        }

        [Theory]
        [InlineData(100f, "A", true)]
        [InlineData(99.9f, "A", true)]
        [InlineData(89.9f, "B", true)]
        [InlineData(79.9f, "C", true)]
        [InlineData(69.9f, "D", false)]
        [InlineData(59.9f, "F", false)]
        public void EvaluateGrade_HandlesDecimalScores_Correctly(float score, string expectedLetterGrade, bool expectedIsPassing)
        {
            // Act
            var result = _gradeEvaluator.EvaluateGrade(score);

            // Assert
            Assert.Equal(expectedLetterGrade, result.letterGrade);
            Assert.Equal(expectedIsPassing, result.isPassing);
        }

        [Fact]
        public void EvaluateGrade_HandlesBoundaryValues_Correctly()
        {
            // Test exact boundary values
            var resultA = _gradeEvaluator.EvaluateGrade(90f);
            Assert.Equal("A", resultA.letterGrade);
            Assert.True(resultA.isPassing);

            var resultB = _gradeEvaluator.EvaluateGrade(80f);
            Assert.Equal("B", resultB.letterGrade);
            Assert.True(resultB.isPassing);

            var resultC = _gradeEvaluator.EvaluateGrade(70f);
            Assert.Equal("C", resultC.letterGrade);
            Assert.True(resultC.isPassing);

            var resultD = _gradeEvaluator.EvaluateGrade(60f);
            Assert.Equal("D", resultD.letterGrade);
            Assert.False(resultD.isPassing);

            // Test just below boundary values
            var resultBelowA = _gradeEvaluator.EvaluateGrade(89.99f);
            Assert.Equal("B", resultBelowA.letterGrade);
            Assert.True(resultBelowA.isPassing);

            var resultBelowB = _gradeEvaluator.EvaluateGrade(79.99f);
            Assert.Equal("C", resultBelowB.letterGrade);
            Assert.True(resultBelowB.isPassing);

            var resultBelowC = _gradeEvaluator.EvaluateGrade(69.99f);
            Assert.Equal("D", resultBelowC.letterGrade);
            Assert.False(resultBelowC.isPassing);

            var resultBelowD = _gradeEvaluator.EvaluateGrade(59.99f);
            Assert.Equal("F", resultBelowD.letterGrade);
            Assert.False(resultBelowD.isPassing);
        }

        [Theory]
        [InlineData(-1f)]
        [InlineData(-10f)]
        [InlineData(-100f)]
        [InlineData(101f)]
        [InlineData(150f)]
        [InlineData(1000f)]
        public void EvaluateGrade_ThrowsArgumentOutOfRangeException_ForInvalidScores(float invalidScore)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _gradeEvaluator.EvaluateGrade(invalidScore));
        }
    }
}
