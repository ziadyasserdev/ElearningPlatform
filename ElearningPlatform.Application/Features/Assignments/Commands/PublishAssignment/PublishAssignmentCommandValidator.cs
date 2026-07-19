using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Commands.PublishAssignment
{
    public class PublishAssignmentCommandValidator
      : AbstractValidator<PublishAssignmentCommand>
    {
        public PublishAssignmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
