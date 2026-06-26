using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Dtos
{
    public class ExamDetailsForStudentDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }

        public int DurationMinutes { get; set; }

        public int TotalScore { get; set; }

        public int PassingScore { get; set; }

        public int QuestionsCount { get; set; }

        public bool HasStarted { get; set; }

        public bool HasCompleted { get; set; }

        public int AttemptsCount { get; set; }
    }
}
