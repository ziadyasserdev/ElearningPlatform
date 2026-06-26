using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.CreateVideoComment
{
    public class CreateVideoCommentCommandValidator : AbstractValidator<CreateVideoCommentCommand>
    {
        public CreateVideoCommentCommandValidator()
        {
            RuleFor(x => x.VideoId)
                .GreaterThan(0)
                .WithMessage("VideoId must be greater than 0");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Comment content is required")
                .MaximumLength(1000)
                .WithMessage("Comment cannot exceed 1000 characters");

            RuleFor(x => x.TimestampSeconds)
                .GreaterThanOrEqualTo(0)
                .When(x => x.TimestampSeconds.HasValue)
                .WithMessage("Timestamp must be non-negative");
        }
    }
}
