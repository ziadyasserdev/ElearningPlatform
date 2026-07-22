using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Dtos
{
    public class MyReviewDto
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? RejectionReason { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? EditedAt { get; set; }
    }
}
