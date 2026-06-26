using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.RestoreInstructor
{
    public class RestoreInstructorCommandValidator
         : AbstractValidator<RestoreInstructorCommand>
    {
        public RestoreInstructorCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
