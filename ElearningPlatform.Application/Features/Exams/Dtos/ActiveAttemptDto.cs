using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Dtos
{
    public class ActiveAttemptDto
    {
        public int AttemptId { get; set; }

        public int ExamId { get; set; }

        public DateTime StartTime { get; set; }

        public int DurationMinutes { get; set; }

        public DateTime EndsAt { get; set; }

        public int RemainingSeconds { get; set; }

        public int AnsweredQuestionsCount { get; set; }

        public int TotalQuestionsCount { get; set; }
    }
}
