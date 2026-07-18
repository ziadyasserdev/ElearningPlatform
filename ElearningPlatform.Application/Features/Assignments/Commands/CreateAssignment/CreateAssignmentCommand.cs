using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Commands.CreateAssignment
{
    public record CreateAssignmentCommand(
     int CourseId,
     string Title,
     string Description,
     int MaxScore,
     DateTime OpenAt,
     DateTime DueDate,
     bool AllowLateSubmission
 ) : IRequest<Result<int>>;
}
