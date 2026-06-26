using ElearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Question : BaseEntity
    {
        public int ExamId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType QuestionType { get; set; }
        public decimal Score { get; set; } = 1m;
        public int OrderIndex { get; set; } = 1;

        public Exam Exam { get; set; } = null!;
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();

        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
