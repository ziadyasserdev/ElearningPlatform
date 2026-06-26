using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Queries.GetTopCategories
{
    public class GetTopCategoriesQueryValidator : AbstractValidator<GetTopCategoriesQuery>
    {
        public GetTopCategoriesQueryValidator()
        {
            RuleFor(x => x.Limit)
                .GreaterThan(0)
                .WithMessage("Limit must be greater than 0.")

                .LessThanOrEqualTo(20)
                .WithMessage("Limit cannot exceed 20 categories.");
        }
    }
}
