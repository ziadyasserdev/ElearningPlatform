using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Dtos
{
    public class EnrollmentAnalyticsDto
    {
        public string Month { get; set; } = string.Empty;

        public int TotalEnrollments { get; set; }
    }
}
