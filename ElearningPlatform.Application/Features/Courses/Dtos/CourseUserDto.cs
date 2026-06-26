using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Dtos
{
    public class CourseUserDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string ThumbnailUrl { get; set; }

        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }

        public string Language { get; set; }
        public string Level { get; set; }

        public decimal AverageRating { get; set; }
        public int TotalStudents { get; set; }

        public string InstructorName { get; set; }
        public string CategoryName { get; set; }
    }
}
