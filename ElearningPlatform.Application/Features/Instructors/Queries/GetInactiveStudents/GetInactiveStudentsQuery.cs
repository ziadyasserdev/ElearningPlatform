using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInactiveStudents
{

    public class GetInactiveStudentsQuery
       : IRequest<Result<List<InactiveStudentDto>>>
    {
        public int CourseId { get; set; }
    }
    public class InactiveStudentDto
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }

        public DateTime EnrolledAt { get; set; }
        public DateTime? LastActiveAt { get; set; }

        public decimal ProgressPercentage { get; set; }
    }
}
