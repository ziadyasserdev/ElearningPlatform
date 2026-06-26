using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Dtos
{
    public class InstructorCourseDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ThumbnailUrl { get; set; }

        public decimal Price { get; set; }

        public int StudentsCount { get; set; }

        public decimal Rating { get; set; }
    }
}
