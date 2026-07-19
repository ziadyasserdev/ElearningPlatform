using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Commands.CloseAssignment
{
    public class CloseAssignmentCommandValidator
      : AbstractValidator<CloseAssignmentCommand>
    {
        public CloseAssignmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
