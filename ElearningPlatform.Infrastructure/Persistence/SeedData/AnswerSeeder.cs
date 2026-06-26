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
    public static class AnswerSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Answers.AnyAsync())
                return;

            var answers = new List<Answer>
        {
            new Answer
            {
                QuestionId = 1,
                AnswerText = "A framework for building web applications",
                IsCorrect = true,
                OrderIndex = 1,
                CreatedAt = DateTime.UtcNow
            },

            new Answer
            {
                QuestionId = 1,
                AnswerText = "Database engine",
                IsCorrect = false,
                OrderIndex = 2,
                CreatedAt = DateTime.UtcNow
            }
        };

            await context.Answers.AddRangeAsync(answers);
            await context.SaveChangesAsync();
        }
    }
}
