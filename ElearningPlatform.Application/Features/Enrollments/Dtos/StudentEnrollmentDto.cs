using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Dtos
{
    public class StudentEnrollmentDto
    {
        public int EnrollmentId { get; set; }

        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public string InstructorName { get; set; } = string.Empty;

        public decimal ProgressPercentage { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime EnrolledAt { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
