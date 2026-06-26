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
    public static class ExamSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Exams.AnyAsync())
                return;

            var exams = new List<Exam>
            {
                new Exam
                {
                    CourseId = 1,
                    Title = "ASP.NET Core Final Exam",
                    Description = "Final assessment for ASP.NET course",
                    DurationMinutes = 60,
                    TotalScore = 100,
                    PassingScore = 60,
                 
                    CreatedAt = DateTime.UtcNow
                },

                new Exam
                {
                    CourseId = 2,
                    Title = "Machine Learning Quiz",
                    Description = "Basic ML concepts",
                    DurationMinutes = 45,
                    TotalScore = 50,
                    PassingScore = 30,
                
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Exams.AddRangeAsync(exams);
            await context.SaveChangesAsync();
        }
    }
}
