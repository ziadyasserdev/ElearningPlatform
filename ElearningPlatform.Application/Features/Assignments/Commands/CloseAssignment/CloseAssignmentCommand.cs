using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Commands.CloseAssignment
{
    public record CloseAssignmentCommand(int Id)
      : IRequest<Result<string>>;
}
