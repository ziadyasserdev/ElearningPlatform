using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class StudentAnswer : BaseEntity
    {
        public int ExamAttemptId { get; set; }
        public int QuestionId { get; set; }     
        public int? SelectedAnswerId { get; set; }
        public string? TextAnswer { get; set; }
        public decimal ScoreAwarded { get; set; }
        public DateTime AnsweredAt { get; set; }

        public ExamAttempt ExamAttempt { get; set; } = null!;
        public Question Question { get; set; } = null!; 
        public Answer? SelectedAnswer { get; set; }

        [NotMapped]
        public bool IsCorrect => SelectedAnswer?.IsCorrect ?? false;
    }
}
