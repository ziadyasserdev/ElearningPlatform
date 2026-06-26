using ElearningPlatform.Application.Features.Sections.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Dtos
{
    public class CourseDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }

        public string InstructorName { get; set; }
        public string CategoryName { get; set; }

        public List<SectionForCourseDto> Sections { get; set; }
    }
}
