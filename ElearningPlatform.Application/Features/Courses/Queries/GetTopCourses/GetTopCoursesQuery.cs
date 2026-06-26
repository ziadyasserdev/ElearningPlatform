using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Queries.GetTopCourses
{
    public class GetTopCoursesQuery
       : IRequest<Result<List<TopCourseDto>>>
    {
        public int Take { get; set; } = 10;
    }
    public class TopCourseDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string InstructorName { get; set; }

        public int TotalEnrollments { get; set; }
        public int ActiveEnrollments { get; set; }
        public int CompletedEnrollments { get; set; }
    }
}
