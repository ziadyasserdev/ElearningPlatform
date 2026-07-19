using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Commands.DeleteAssignmentAttachment
{
    public class DeleteAssignmentAttachmentCommandValidator
        : AbstractValidator<DeleteAssignmentAttachmentCommand>
    {
        public DeleteAssignmentAttachmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
