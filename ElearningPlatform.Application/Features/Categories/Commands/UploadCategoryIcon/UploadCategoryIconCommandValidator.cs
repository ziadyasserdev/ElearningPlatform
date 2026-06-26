using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.UploadCategoryIcon
{
    public class UploadCategoryIconCommandValidator : AbstractValidator<UploadCategoryIconCommand>
    {
        public UploadCategoryIconCommandValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0);

            RuleFor(x => x.FormFile)
     .Cascade(CascadeMode.Stop)
     .NotNull().WithMessage("Icon file is required.")
     .Must(file => file.Length > 0)
     .WithMessage("Icon file cannot be empty.");
        }
    }
}
