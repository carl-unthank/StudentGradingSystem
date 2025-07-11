using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentGrading.Api.Services.Interfaces;
using StudentGrading.Api.Models;

namespace StudentGrading.Api.Services.Implementations
{
    public class GradeEvaluator : IGradeEvaluator
    {
        private readonly List<GradeBoundary> _gradeBoundaries;
        private readonly double _minimumPassingScore;

        public GradeEvaluator(IEnumerable<GradeBoundary> gradeBoundaries, double minimumPassingScore = 70f)
        {
            if (gradeBoundaries == null || !gradeBoundaries.Any())
            {
                throw new ArgumentException("Grade boundaries collection cannot be null or empty.", nameof(gradeBoundaries));
            }

            if (minimumPassingScore < 0f || minimumPassingScore > 100f)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumPassingScore), "Minimum passing score must be between 0 and 100.");
            }

            var duplicateLetterGrades = gradeBoundaries
                .GroupBy(g => g.LetterGrade, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);

            if (duplicateLetterGrades.Any())
            {
                throw new ArgumentException($"Duplicate letter grades found: {string.Join(", ", duplicateLetterGrades)}", nameof(gradeBoundaries));
            }

            _gradeBoundaries = gradeBoundaries.OrderByDescending(b => b.MinimumPercentage).ToList();
            _minimumPassingScore = minimumPassingScore;
        }

        public (string letterGrade, bool isPassing) EvaluateGrade(double percentage)
        {
            if (percentage < 0f || percentage > 100f)
            {
                throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 0 and 100.");
            }

            var determinedLetterGrade = "F";

            foreach (var boundary in _gradeBoundaries)
            {
                if (percentage >= boundary.MinimumPercentage)
                {
                    determinedLetterGrade = boundary.LetterGrade;
                    break;
                }
            }

            var determinedIsPassing = percentage >= _minimumPassingScore;

            return (determinedLetterGrade, determinedIsPassing);
        }
    }
}