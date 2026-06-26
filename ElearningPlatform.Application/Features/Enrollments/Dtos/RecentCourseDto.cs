using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Dtos
{
    public class RecentCourseDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseThumbnail { get; set; }

        public int LastVideoId { get; set; }
        public string LastVideoTitle { get; set; }

        public double ProgressPercentage { get; set; }
        public DateTime LastWatchedAt { get; set; }
    }
}
