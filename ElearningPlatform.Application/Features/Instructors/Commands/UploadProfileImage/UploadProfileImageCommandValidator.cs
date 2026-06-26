using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.UploadProfileImage
{
    public class UploadProfileImageCommandValidator
    : AbstractValidator<UploadProfileImageCommand>
    {
        public UploadProfileImageCommandValidator()
        {
            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image is required.");

            RuleFor(x => x.Image.Length)
                .LessThanOrEqualTo(2 * 1024 * 1024)
                .WithMessage("Image size must not exceed 2MB.");

            RuleFor(x => x.Image.ContentType)
                .Must(BeValidImageType)
                .WithMessage("Only JPG, JPEG, or PNG images are allowed.");
        }

        private bool BeValidImageType(string contentType)
        {
            var allowedTypes = new[]
            {
            "image/jpeg",
            "image/jpg",
            "image/png"
        };

            return allowedTypes.Contains(contentType);
        }
    }
}
