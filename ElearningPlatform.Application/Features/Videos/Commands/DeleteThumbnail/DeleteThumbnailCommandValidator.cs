using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.DeleteThumbnail
{
    public class DeleteThumbnailCommandValidator : AbstractValidator<DeleteThumbnailCommand>
    {
        public DeleteThumbnailCommandValidator()
        {
            RuleFor(x => x.VideoId)
                .GreaterThan(0)
                .WithMessage("VideoId must be greater than 0");
        }
    }
}
