using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.UpdateCourseThumbnail
{
    public class UpdateCourseThumbnailCommandValidator
        : AbstractValidator<UpdateCourseThumbnailCommand>
    {
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

        public UpdateCourseThumbnailCommandValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("CourseId must be greater than 0.");

            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("Thumbnail file is required.");

            RuleFor(x => x.File)
                .Must(file => file != null && file.Length > 0)
                .WithMessage("File cannot be empty.");

            RuleFor(x => x.File)
                .Must(file =>
                {
                    if (file == null) return false;

                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    return allowedExtensions.Contains(ext);
                })
                .WithMessage("Only .jpg, .jpeg, .png, .webp files are allowed.");

            RuleFor(x => x.File)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Max file size is 5MB.");
        }
    }
}
