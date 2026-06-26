using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.EditCategoryIcon
{
    public class EditCategoryIconCommandValidator : AbstractValidator<EditCategoryIconCommand>
    {
        public EditCategoryIconCommandValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0);

            RuleFor(x => x.FormFile)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Icon file is required.")
                .Must(file => file.Length > 0)
                .WithMessage("Icon file cannot be empty.")
                .Must(file => file.Length <= 2 * 1024 * 1024)
                .WithMessage("Icon size must not exceed 2MB.")
                .Must(file =>
                    file.ContentType == "image/png" ||
                    file.ContentType == "image/jpeg" ||
                    file.ContentType == "image/webp")
                .WithMessage("Only PNG, JPG, or WEBP images are allowed.");
        }
    }
}
