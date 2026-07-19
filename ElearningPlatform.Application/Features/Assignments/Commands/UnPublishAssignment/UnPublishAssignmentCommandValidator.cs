using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Commands.UnPublishAssignment
{
    public class UnPublishAssignmentCommandValidator
     : AbstractValidator<UnPublishAssignmentCommand>
    {
        public UnPublishAssignmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
