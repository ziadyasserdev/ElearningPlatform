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
    public static class CourseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Courses.AnyAsync())
                return;

            var courses = new List<Course>
            {
                new Course
                {
                    Title = "ASP.NET Core Web API",
                    Slug = "aspnet-core-api",
                    Description = "Learn how to build REST APIs using ASP.NET Core",
                    ShortDescription = "ASP.NET Core API course",
                    Price = 50,
                    Level = CourseLevel.Intermediate,
                    Language = "English",
                    ThumbnailUrl = "images/api.png",
                    CategoryId = 1,
                    InstructorId =7,
                    Status = CourseStatus.Published,
                    TotalDurationInMinutes = 600,
                    TotalLessons = 20,
                    AverageRating = 4.5m,
                    TotalStudents = 120,
                    PublishedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                },

                new Course
                {
                    Title = "Machine Learning Basics",
                    Slug = "machine-learning",
                    Description = "Introduction to Machine Learning",
                    ShortDescription = "ML fundamentals",
                    Price = 70,
                    Level = CourseLevel.Beginner,
                    Language = "English",
                    ThumbnailUrl = "images/ml.png",
                    CategoryId = 2,
                    InstructorId = 7,
                    Status = CourseStatus.Published,
                    TotalDurationInMinutes = 800,
                    TotalLessons = 25,
                    AverageRating = 4.7m,
                    TotalStudents = 90,
                    PublishedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();
        }
    }
}
