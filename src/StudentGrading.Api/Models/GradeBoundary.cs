using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGrading.Api.Models
{
    public class GradeBoundary
    {
        public double MinimumPercentage { get; init; }
        public string LetterGrade { get; init; } = string.Empty;

        public GradeBoundary(double minimumPercentage, string letterGrade)
        {
            if (minimumPercentage < 0f || minimumPercentage > 100f)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumPercentage), "Minimum percentage must be between 0 and 100.");
            }

            if (string.IsNullOrWhiteSpace(letterGrade))
            {
                throw new ArgumentException("Letter grade cannot be null or empty.", nameof(letterGrade));
            }

            MinimumPercentage = minimumPercentage;
            LetterGrade = letterGrade;
        }
    }
}