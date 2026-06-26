using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Dtos
{
    public class ExamStatisticsDto
    {
        public int ExamId { get; set; }

        public int TotalAttempts { get; set; }

        public int CompletedAttempts { get; set; }

        public int PassedStudents { get; set; }

        public int FailedStudents { get; set; }

        public double AverageScore { get; set; }

        public double PassRate { get; set; }
    }
}
