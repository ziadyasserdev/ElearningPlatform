using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Commands.ReopenAssignment
{
    public class ReopenAssignmentCommandValidator
        : AbstractValidator<ReopenAssignmentCommand>
    {
        public ReopenAssignmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
