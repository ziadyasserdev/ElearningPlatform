using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Dtos
{
    public class TopCategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? IconUrl { get; set; }

        public int CourseCount { get; set; }
    }
}
