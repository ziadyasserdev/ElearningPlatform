using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Commands.ExtendAssignmentDeadline
{
    public class ExtendAssignmentDeadlineCommandValidator
       : AbstractValidator<ExtendAssignmentDeadlineCommand>
    {
        public ExtendAssignmentDeadlineCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.NewDueDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("New due date must be in the future.");
        }
    }
}
