using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Models;
using ElearningPlatform.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Infrastructure.Persistence.SeedData
{
    public static class QuestionSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Questions.AnyAsync())
                return;

            var questions = new List<Question>
        {
            new Question
            {
                ExamId = 1,
                QuestionText = "What is ASP.NET Core?",
                QuestionType = QuestionType.MCQ,
                Score = 5,
                OrderIndex = 1,
                CreatedAt = DateTime.UtcNow
            },

            new Question
            {
                ExamId = 1,
                QuestionText = "Which protocol does Web API use?",
                QuestionType = QuestionType.MCQ,
                Score = 5,
                OrderIndex = 2,
                CreatedAt = DateTime.UtcNow
            }
        };

            await context.Questions.AddRangeAsync(questions);
            await context.SaveChangesAsync();
        }
    }
}
