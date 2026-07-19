using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Dtos
{

    public class AssignmentStatisticsDto
    {
        public int TotalStudents { get; set; }

        public int SubmittedStudents { get; set; }

        public int PendingStudents { get; set; }

        public int LateSubmissions { get; set; }

        public decimal AverageScore { get; set; }

        public int HighestScore { get; set; }

        public int LowestScore { get; set; }

        public decimal SubmissionRate { get; set; }
    }

}
