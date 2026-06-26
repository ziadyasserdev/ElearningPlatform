using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.ToggleInstructorStatus
{
    public class ToggleInstructorStatusCommandValidator
        : AbstractValidator<ToggleInstructorStatusCommand>
    {
        public ToggleInstructorStatusCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
