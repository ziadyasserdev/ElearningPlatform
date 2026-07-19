using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Commands.RenameAssignmentAttachment
{
    public class RenameAssignmentAttachmentCommandValidator
     : AbstractValidator<RenameAssignmentAttachmentCommand>
    {
        public RenameAssignmentAttachmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.FileName)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
