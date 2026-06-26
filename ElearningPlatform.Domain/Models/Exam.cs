using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Exam : BaseEntity
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int DurationMinutes { get; set; } = 30;

        public int TotalScore { get; set; } = 100;

        public int PassingScore { get; set; } = 50;

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }


        public Course Course { get; set; } = null!;

        public ICollection<Question> Questions { get; set; } = new List<Question>();

        public ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();
    }
}
