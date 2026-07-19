using ElearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Dtos
{
    public class LateSubmissionDto
    {
        public int SubmissionId { get; set; }

        public string StudentId { get; set; } = string.Empty;

        public string StudentName { get; set; } = string.Empty;

        public string? ProfilePictureUrl { get; set; }

        public string FileUrl { get; set; } = string.Empty;

        public int? Score { get; set; }

        public SubmissionStatus Status { get; set; }

        public DateTime SubmittedAt { get; set; }

        public DateTime DueDate { get; set; }

        public int LateDays { get; set; }
    }
}
