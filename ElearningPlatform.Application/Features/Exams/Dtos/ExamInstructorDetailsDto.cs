using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Dtos
{
    public class ExamInstructorDetailsDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }

        public int DurationMinutes { get; set; }

        public int TotalScore { get; set; }

        public int PassingScore { get; set; }

       

      
        public int TotalAttempts { get; set; }

        public int CompletedAttempts { get; set; }

        public int PassedStudents { get; set; }

        public int FailedStudents { get; set; }

        public double AverageScore { get; set; }

        public int QuestionsCount { get; set; }
    }
}
