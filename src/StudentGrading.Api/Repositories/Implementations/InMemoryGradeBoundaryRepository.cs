using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentGrading.Api.Repositories.Interfaces;
using StudentGrading.Api.Models;

namespace StudentGrading.Api.Repositories.Implementations
{
    public class InMemoryGradeBoundaryRepository : IGradeBoundaryRepository
    {
        private static readonly List<GradeBoundary> _gradeBoundaries = new()
        {
            new GradeBoundary(95f, "A+"),
            new GradeBoundary(90f, "A"),
            new GradeBoundary(85f, "B+"),
            new GradeBoundary(80f, "B"),
            new GradeBoundary(75f, "C+"),
            new GradeBoundary(70f, "C"),
            new GradeBoundary(65f, "D+"),
            new GradeBoundary(60f, "D"),
            new GradeBoundary(55f, "E+"),
            new GradeBoundary(50f, "E"),
            new GradeBoundary(0f, "F")
        };

        public Task<IEnumerable<GradeBoundary>> GetAllGradeBoundariesAsync()
        {
            return Task.FromResult(_gradeBoundaries.AsEnumerable());
        }
    }
}