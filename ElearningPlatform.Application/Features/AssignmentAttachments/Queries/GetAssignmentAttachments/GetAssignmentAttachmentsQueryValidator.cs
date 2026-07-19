using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Queries.GetAssignmentAttachments
{
    public class GetAssignmentAttachmentsQueryValidator
        : AbstractValidator<GetAssignmentAttachmentsQuery>
    {
        public GetAssignmentAttachmentsQueryValidator()
        {
            RuleFor(x => x.AssignmentId)
                .GreaterThan(0)
                .WithMessage("Assignment Id must be greater than zero.");
        }
    }
}
