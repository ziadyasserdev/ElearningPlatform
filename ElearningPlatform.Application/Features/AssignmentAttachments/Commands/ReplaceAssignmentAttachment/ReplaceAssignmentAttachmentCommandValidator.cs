using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Commands.ReplaceAssignmentAttachment
{
    public class ReplaceAssignmentAttachmentCommandValidator
         : AbstractValidator<ReplaceAssignmentAttachmentCommand>
    {
        public ReplaceAssignmentAttachmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Attachment Id must be greater than zero.");

            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required.");

            RuleFor(x => x.File.Length)
                .GreaterThan(0)
                .When(x => x.File != null)
                .WithMessage("File cannot be empty.");

            RuleFor(x => x.File.Length)
                .LessThanOrEqualTo(20 * 1024 * 1024)
                .When(x => x.File != null)
                .WithMessage("File size must not exceed 20 MB.");
        }
    }
}
