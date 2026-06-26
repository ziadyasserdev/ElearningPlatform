using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.CreateVideo
{

    public class CreateVideoCommandValidator : AbstractValidator<CreateVideoCommand>
    {
        public CreateVideoCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200);

            RuleFor(x => x.LessonId)
                .GreaterThan(0).WithMessage("LessonId must be valid");

            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required")
                .Must(BeValidFile).WithMessage("Invalid file");
            RuleFor(x => x.Duration)
    .GreaterThan(0)
    .WithMessage("Duration must be greater than 0");

            RuleFor(x => x.File)
                .Must(BeValidExtension).WithMessage("Only video files are allowed");
        }

        private bool BeValidFile(IFormFile file)
        {
            return file != null && file.Length > 0;
        }

        private bool BeValidExtension(IFormFile file)
        {
            if (file == null) return false;

            var ext = Path.GetExtension(file.FileName).ToLower();

            return ext == ".mp4" || ext == ".mov" || ext == ".mkv";
        }
    }
}
