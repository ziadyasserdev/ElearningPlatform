using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Dtos
{
    public class CourseStudentDto
    {
        public string StudentId { get; set; } = null!;

        public string StudentName { get; set; } = string.Empty;

        public string StudentEmail { get; set; } = string.Empty;

        public decimal ProgressPercentage { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime EnrolledAt { get; set; }
    }
}
