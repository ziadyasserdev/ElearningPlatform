using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.BulkRestoreCategory
{
    public class BulkRestoreCategoryCommandValidator
         : AbstractValidator<BulkRestoreCategoryCommand>
    {
        public BulkRestoreCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryIds)
                .NotEmpty().WithMessage("CategoryIds cannot be empty.");

            RuleForEach(x => x.CategoryIds)
                .GreaterThan(0);
        }
    }
}
