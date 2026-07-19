using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Commands.RestoreAssignment
{
    public class RestoreAssignmentCommandValidator
    : AbstractValidator<RestoreAssignmentCommand>
    {
        public RestoreAssignmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
