using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Submission : BaseEntity
    {
        public int AssignmentId { get; set; }

        public string StudentId { get; set; } = null!;

      
        public string FileName { get; set; } = string.Empty;

       
        public string FileUrl { get; set; } = string.Empty;

     
        public string ContentType { get; set; } = string.Empty;

       
        public long FileSize { get; set; }

      
        public string? Comment { get; set; }

        
        public int? Score { get; set; }

     
        public string? Feedback { get; set; }

        public DateTime SubmittedAt { get; set; }

        public DateTime? GradedAt { get; set; }
        public int AttemptNumber { get; set; } = 1;

        public DateTime? ReturnedAt { get; set; }

        public string? ReturnReason { get; set; }

        public bool IsLate { get; set; }

        public SubmissionStatus Status { get; set; }

        public Assignment Assignment { get; set; } = null!;

        public ApplicationUser Student { get; set; } = null!;
    }
}
