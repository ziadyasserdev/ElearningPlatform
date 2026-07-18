using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Commands.UpdateAssignment
{
    public record UpdateAssignmentCommand(
      int Id,
      string Title,
      string Description,
      int MaxScore,
      DateTime OpenAt,
      DateTime DueDate,
      bool AllowLateSubmission
  ) : IRequest<Result<string>>;
}
