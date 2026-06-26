using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.UpdateProfileImage
{
    public class UpdateProfileImageCommandValidator : AbstractValidator<UpdateProfileImageCommand>
    {
        private readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long maxFileSize = 5 * 1024 * 1024; // 5 MB

        public UpdateProfileImageCommandValidator()
        {
            RuleFor(x => x.formFile)
                .NotNull()
                .WithMessage("Profile image is required.")
                .Must(IsValidFileType)
                .WithMessage("Invalid file type. Only jpg, jpeg, png, gif are allowed.")
                .Must(f => f.Length <= maxFileSize)
                .WithMessage("File size must not exceed 5 MB.");
        }

        private bool IsValidFileType(IFormFile file)
        {
            if (file == null) return false;
            var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
            return permittedExtensions.Contains(extension);
        }
    }
}
