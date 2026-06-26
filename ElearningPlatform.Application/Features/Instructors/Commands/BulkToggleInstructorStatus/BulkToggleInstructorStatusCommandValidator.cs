using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.BulkToggleInstructorStatus
{
    public class BulkToggleInstructorStatusCommandValidator : AbstractValidator<BulkToggleInstructorStatusCommand>
    {
        public BulkToggleInstructorStatusCommandValidator()
        {
            RuleFor(x => x.UserIds)
                .NotEmpty()
                .WithMessage("You must provide at least one instructor ID.")
              ;

          
            RuleFor(x => x.UserIds.Count)
                .LessThanOrEqualTo(100)
                .WithMessage("You cannot update more than 100 instructors at once.");
        }
    }
}
