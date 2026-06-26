using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.UploadCourseThumbnail
{

    public class UploadCourseThumbnailCommandValidator
     : AbstractValidator<UploadCourseThumbnailCommand>
    {
        private readonly string[] allowedExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        public UploadCourseThumbnailCommandValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("CourseId must be greater than zero.");

            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("Thumbnail file is required.");

            RuleFor(x => x.File)
                .Must(file => file != null && file.Length > 0)
                .WithMessage("Thumbnail file cannot be empty.");

            RuleFor(x => x.File)
                .Must(file =>
                {
                    if (file == null)
                        return false;

                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    return allowedExtensions.Contains(extension);
                })
                .WithMessage("Only .jpg, .jpeg, .png and .webp files are allowed.");

            RuleFor(x => x.File)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Thumbnail size must not exceed 5 MB.");
        }
    }
}
