using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetStudentsInCourse
{
    public class GetStudentsInCourseQuery : IRequest<Result<List<CourseStudentDto>>>
    {
        public int CourseId { get; set; }
    }
    public class CourseStudentDto
    {
        public string StudentName { get; set; }
        public string Email { get; set; }

        public decimal Progress { get; set; }

        public DateTime EnrolledAt { get; set; }
    }
}
