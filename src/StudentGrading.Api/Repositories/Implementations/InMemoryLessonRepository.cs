using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentGrading.Api.Repositories.Interfaces;
using StudentGrading.Api.Models;

namespace StudentGrading.Api.Repositories.Implementations
{
    public class InMemoryLessonRepository : ILessonRepository
    {
        private static readonly List<Lesson> _lessons = new()
        {
            new Lesson { Id = 1, Name = "Maths" },
            new Lesson { Id = 2, Name = "English" },
            new Lesson { Id = 3, Name = "Science" },
            new Lesson { Id = 4, Name = "History" },
            new Lesson { Id = 5, Name = "Geography" },
            new Lesson { Id = 6, Name = "Art" },
            new Lesson { Id = 7, Name = "Music" },
            new Lesson { Id = 8, Name = "PE" },
            new Lesson { Id = 9, Name = "ICT" },
            new Lesson { Id = 10, Name = "French" },
        };

        public async Task<IEnumerable<Lesson>> GetAllLessonsAsync()
        {
            return await Task.FromResult(_lessons.AsEnumerable());
        }

        public async Task<Lesson> GetLessonByIdAsync(int lessonId)
        {
            var lesson = _lessons.FirstOrDefault(l => l.Id == lessonId);
            return await Task.FromResult(lesson ?? throw new ArgumentException($"Lesson with ID {lessonId} not found."));
        }
    }
}