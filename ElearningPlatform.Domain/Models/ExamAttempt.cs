using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class ExamAttempt : BaseEntity
    {
        public string StudentId { get; set; }

        public int ExamId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public int Score { get; set; }

        public decimal Percentage { get; set; }

        public bool IsPassed { get; set; }

        public ExamAttemptStatus Status { get; set; }

        public int AttemptNumber { get; set; }

        public ApplicationUser Student { get; set; } = null!;

        public Exam Exam { get; set; } = null!;

        public ICollection<StudentAnswer> StudentAnswers { get; set; }
            = new List<StudentAnswer>();
    }
}
