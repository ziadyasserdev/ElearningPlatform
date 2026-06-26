using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Enrollments.Queries.AdminGetAllEnrollments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetEnrollmentById
{
    public class GetEnrollmentByIdQuery
        : IRequest<Result<AdminEnrollmentDto>>
    {
        public int Id { get; set; }
    }
}
