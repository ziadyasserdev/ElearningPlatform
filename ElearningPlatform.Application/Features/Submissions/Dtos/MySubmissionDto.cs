using ElearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Dtos
{
    public class MySubmissionDto
    {
        public int Id { get; set; }

        public int AssignmentId { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FileUrl { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string? Comment { get; set; }

        public int? Score { get; set; }

        public string? Feedback { get; set; }

        public bool IsLate { get; set; }

        public string Status { get; set; }

        public DateTime SubmittedAt { get; set; }

        public DateTime? GradedAt { get; set; }
        public string AssignmentTitle { get; set; }

        public DateTime DueDate { get; set; }

        public int MaxScore { get; set; }
        public bool IsGraded => Status == SubmissionStatus.Graded.ToString();
    }
}
