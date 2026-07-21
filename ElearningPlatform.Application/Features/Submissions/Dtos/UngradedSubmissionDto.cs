using ElearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Dtos
{
    public class UngradedSubmissionDto
    {
        public int Id { get; set; }

        public int AssignmentId { get; set; }

        public string AssignmentTitle { get; set; } = string.Empty;

        public string StudentId { get; set; } = string.Empty;

        public string StudentName { get; set; } = string.Empty;

        public string? StudentEmail { get; set; }

        public string FileName { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string? Comment { get; set; }

        public bool IsLate { get; set; }

    

        public DateTime SubmittedAt { get; set; }
    }
}
