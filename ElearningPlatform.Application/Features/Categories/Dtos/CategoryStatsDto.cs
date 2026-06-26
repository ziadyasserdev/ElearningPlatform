using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Dtos
{
    public class CategoryStatsDto
    {
        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
        public decimal AverageRating { get; set; }
    }
}
