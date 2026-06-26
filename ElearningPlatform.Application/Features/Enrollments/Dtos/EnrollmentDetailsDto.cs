using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Dtos
{
    public class EnrollmentDetailsDto
    {
        public int CourseId { get; set; }

        public string CourseTitle { get; set; }

        public string InstructorName { get; set; }

        public string CategoryName { get; set; }

        public string ThumbnailUrl { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public decimal ProgressPercentage { get; set; }

        public string Status { get; set; }

        public DateTime EnrolledAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int TotalLessons { get; set; }

        public int TotalVideos { get; set; }

        public int CompletedVideos { get; set; }
    }
}
