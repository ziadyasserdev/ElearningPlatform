using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.ReplyVideoComment
{
    public class ReplyVideoCommentCommandValidator : AbstractValidator<ReplyVideoCommentCommand>
    {
        public ReplyVideoCommentCommandValidator()
        {
            RuleFor(x => x.VideoId)
                .GreaterThan(0)
                .WithMessage("VideoId must be greater than 0");

            RuleFor(x => x.CommentId)
                .GreaterThan(0)
                .WithMessage("CommentId must be greater than 0");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Reply content is required")
                .MaximumLength(1000)
                .WithMessage("Reply cannot exceed 1000 characters");
        }
    }
}
