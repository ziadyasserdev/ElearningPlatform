using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.BulkDeleteCategory
{
    public class BulkDeleteCategoryCommandValidator : AbstractValidator<BulkDeleteCategoryCommand>
    {
        public BulkDeleteCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryIds)
                .NotEmpty().WithMessage("CategoryIds list cannot be empty.");

            RuleForEach(x => x.CategoryIds)
                .GreaterThan(0);
        }
    }
}
