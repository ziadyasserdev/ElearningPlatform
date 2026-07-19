using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Queries.DownloadAssignmentAttachment
{
    public class DownloadAssignmentAttachmentQueryValidator
       : AbstractValidator<DownloadAssignmentAttachmentQuery>
    {
        public DownloadAssignmentAttachmentQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Attachment Id must be greater than zero.");
        }
    }

}
