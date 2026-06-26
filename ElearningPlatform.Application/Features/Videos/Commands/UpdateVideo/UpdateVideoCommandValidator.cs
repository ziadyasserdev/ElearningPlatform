using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.UpdateVideo
{

    public class UpdateVideoCommandValidator : AbstractValidator<UpdateVideoCommand>
    {
        public UpdateVideoCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid video id");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required")
                .MaximumLength(200)
                .WithMessage("Title must not exceed 200 characters");


            When(x => x.File != null, () =>
            {
                RuleFor(x => x.File.Length)
                    .LessThanOrEqualTo(500 * 1024 * 1024) // 500 MB
                    .WithMessage("File size must not exceed 500 MB");

                RuleFor(x => x.File.ContentType)
                    .Must(type => type.StartsWith("video/"))
                    .WithMessage("Only video files are allowed");
            });
        }
    }
}
