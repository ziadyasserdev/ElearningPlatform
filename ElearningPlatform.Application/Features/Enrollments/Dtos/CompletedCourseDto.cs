using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Dtos
{
    public class CompletedCourseDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseThumbnail { get; set; }

        public decimal ProgressPercentage { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
