using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Queries.CheckCategoryNameAvailability
{
    public class CheckCategoryNameAvailabilityQueryValidator
        : AbstractValidator<CheckCategoryNameAvailabilityQuery>
    {
        public CheckCategoryNameAvailabilityQueryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")

                .MaximumLength(100)
                .WithMessage("Category name must not exceed 100 characters.");
        }
    }
}
