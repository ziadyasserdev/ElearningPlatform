using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Dtos
{
    public class TopStudentDto
    {
        public string StudentId { get; set; } = string.Empty;

        public string StudentName { get; set; } = string.Empty;

        public string? ProfilePictureUrl { get; set; }

        public int? Score { get; set; }

        public DateTime SubmittedAt { get; set; }

        public bool IsLate { get; set; }
    }
}
