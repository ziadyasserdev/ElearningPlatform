using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Commands.DeleteAssignment
{
    public class DeleteAssignmentCommandValidator
      : AbstractValidator<DeleteAssignmentCommand>
    {
        public DeleteAssignmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
