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
    public static class CategorySeeder
    {
       
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Categories.AnyAsync())
                return;

            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Programming",
                    Slug = "programming",
                    Description = "Programming courses",
                    IconUrl = "icons/programming.png",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },

                new Category
                {
                    Name = "Data Science",
                    Slug = "data-science",
                    Description = "Data Science and AI courses",
                    IconUrl = "icons/data.png",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },

                new Category
                {
                    Name = "Cyber Security",
                    Slug = "cyber-security",
                    Description = "Cyber Security courses",
                    IconUrl = "icons/security.png",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }
    }
}
