using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.BulkToggleCategoryStatus
{
    public class BulkToggleCategoryStatusCommandValidator
       : AbstractValidator<BulkToggleCategoryStatusCommand>
    {
        public BulkToggleCategoryStatusCommandValidator()
        {
            RuleFor(x => x.CategoryIds)
                .NotEmpty().WithMessage("CategoryIds cannot be empty.");

            RuleForEach(x => x.CategoryIds)
                .GreaterThan(0);

            RuleFor(x => x.IsActive)
                .NotNull();
        }
    }
}
