using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.UpdateVideoComment
{
    public class UpdateVideoCommentCommandValidator : AbstractValidator<UpdateVideoCommentCommand>
    {
        public UpdateVideoCommentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Comment Id must be greater than 0");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Comment content is required")
                .MaximumLength(1000)
                .WithMessage("Comment cannot exceed 1000 characters");
        }
    }
}
