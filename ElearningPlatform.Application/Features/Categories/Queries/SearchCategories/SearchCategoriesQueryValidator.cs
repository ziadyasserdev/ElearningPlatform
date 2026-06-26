using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Queries.SearchCategories
{
    public class SearchCategoriesQueryValidator : AbstractValidator<SearchCategoriesQuery>
    {
        public SearchCategoriesQueryValidator()
        {
           
            RuleFor(x => x.SearchTerm)
                .NotEmpty()
                .WithMessage("Search term is required.")
                .MaximumLength(100)
                .WithMessage("Search term must not exceed 100 characters.");

          
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than zero.");

        
            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than zero.")
                .LessThanOrEqualTo(50)
                .WithMessage("Page size must not exceed 50.");
        }
    }
}
