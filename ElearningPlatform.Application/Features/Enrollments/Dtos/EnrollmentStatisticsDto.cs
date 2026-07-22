using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Dtos
{
    public class EnrollmentStatisticsDto
    {
        public int TotalEnrollments { get; set; }

        public int ActiveEnrollments { get; set; }

        public int CompletedEnrollments { get; set; }

        public int CancelledEnrollments { get; set; }

        public decimal CompletionRate { get; set; }
    }
}
