using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Enrollments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetStudentEnrollments
{
    public class GetStudentEnrollmentsQuery
         : IRequest<Result<PaginatedResult<StudentEnrollmentDto>>>
    {
        public string StudentId { get; set; } = string.Empty;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
