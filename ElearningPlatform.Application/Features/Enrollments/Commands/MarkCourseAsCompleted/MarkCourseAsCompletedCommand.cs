using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Commands.MarkCourseAsCompleted
{
    public class MarkCourseAsCompletedCommand
         : IRequest<Result<string>>
    {
        public int CourseId { get; set; }
    }
}
