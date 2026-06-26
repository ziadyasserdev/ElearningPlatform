using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Dtos
{
    public class CourseProgressDto
    {
        public int CourseId { get; set; }

        public decimal ProgressPercentage { get; set; }

        public int TotalVideos { get; set; }

        public int CompletedVideos { get; set; }

        public int RemainingVideos { get; set; }

        public bool IsCompleted { get; set; }
    }
}
