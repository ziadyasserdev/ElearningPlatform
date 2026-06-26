using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Dtos
{
    public class InstructorDashboardDto
    {
        public int InstructorId { get; set; }
        public string FullName { get; set; }

        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }

        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}
