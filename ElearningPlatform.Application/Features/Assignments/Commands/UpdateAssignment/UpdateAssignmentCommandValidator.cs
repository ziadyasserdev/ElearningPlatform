using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Commands.UpdateAssignment
{
    public class UpdateAssignmentCommandValidator
     : AbstractValidator<UpdateAssignmentCommand>
    {
        public UpdateAssignmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(3000);

            RuleFor(x => x.MaxScore)
                .GreaterThan(0);

            RuleFor(x => x.OpenAt)
                .LessThan(x => x.DueDate);

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow);
        }
    }
}
