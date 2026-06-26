using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Dtos
{
    public class ExamResultDto
    {
        public string StudentName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int Score { get; set; }

        public bool Passed { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
