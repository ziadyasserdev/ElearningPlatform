using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Dtos
{
    public class ContinueWatchingDto
    {
        public int CourseId { get; set; }

        public string CourseTitle { get; set; }

        public string CourseThumbnail { get; set; }

        public int VideoId { get; set; }

        public string VideoTitle { get; set; }

        public int LastWatchedSecond { get; set; }

        public double ProgressPercentage { get; set; }
    }
}
