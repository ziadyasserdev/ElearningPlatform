using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.UpdateThumbnail
{
    public class UpdateThumbnailCommandValidator:AbstractValidator<UpdateThumbnailCommand>
    {
        public UpdateThumbnailCommandValidator()
        {
            RuleFor(x => x.VideoId)
              .GreaterThan(0)
              .WithMessage("Invalid Video Id");

            RuleFor(x => x.Thumbnail)
                .NotNull()
                .WithMessage("Thumbnail is required");

            When(x => x.Thumbnail != null, () =>
            {
                RuleFor(x => x.Thumbnail.Length)
                    .LessThanOrEqualTo(5 * 1024 * 1024)
                    .WithMessage("Thumbnail max size is 5MB");

                RuleFor(x => x.Thumbnail.ContentType)
                    .Must(x => x.StartsWith("image/"))
                    .WithMessage("Only image files are allowed");
            });
        }
    }
}
