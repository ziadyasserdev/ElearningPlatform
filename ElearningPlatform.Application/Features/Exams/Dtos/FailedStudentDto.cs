using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Dtos
{
    public class FailedStudentDto
    {
        public string StudentName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int Score { get; set; }

        public DateTime? EndTime { get; set; }
    }
}