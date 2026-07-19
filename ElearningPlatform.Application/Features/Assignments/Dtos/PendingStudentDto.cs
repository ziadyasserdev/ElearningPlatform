using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Dtos
{
    public class PendingStudentDto
    {
        public string StudentId { get; set; } = string.Empty;

        public string StudentName { get; set; } = string.Empty;

        public string? ProfilePictureUrl { get; set; }

        public DateTime EnrolledAt { get; set; }

        public decimal ProgressPercentage { get; set; }
    }
}
