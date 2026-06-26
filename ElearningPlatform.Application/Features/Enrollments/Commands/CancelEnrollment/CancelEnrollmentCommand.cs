using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Commands.CancelEnrollment
{
    public class CancelEnrollmentCommand : IRequest<Result<string>>
    {
        public int CourseId { get; set; }
    }
}
