using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetTopStudents
{
    public class GetTopStudentsQuery
       : IRequest<Result<List<TopStudentDto>>>
    {
        public int CourseId { get; set; }
    }
    public class TopStudentDto
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }

        public decimal ProgressPercentage { get; set; }
        public double CompletionScore { get; set; }

        public DateTime EnrolledAt { get; set; }
    }
}
