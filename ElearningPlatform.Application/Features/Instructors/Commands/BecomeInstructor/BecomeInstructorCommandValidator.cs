using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.BecomeInstructor
{
    public class BecomeInstructorCommandValidator : AbstractValidator<BecomeInstructorCommand>
    {
        public BecomeInstructorCommandValidator()
        {
            RuleFor(x => x.Bio)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.Specialization)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.ExperienceYears)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(50);
        }
    }
}
