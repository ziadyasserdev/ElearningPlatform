using ElearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Dtos
{
    public class MySubmissionListDto
    {
        public int Id { get; set; }

        public int AssignmentId { get; set; }

        public string AssignmentTitle { get; set; } = string.Empty;

        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public int? Score { get; set; }

        public bool IsLate { get; set; }

        public SubmissionStatus Status { get; set; }

        public DateTime SubmittedAt { get; set; }

        public DateTime? GradedAt { get; set; }
    }
}
