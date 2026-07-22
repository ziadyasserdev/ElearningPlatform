using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.SearchEnrollments
{
    public class SearchEnrollmentsQueryValidator
        : AbstractValidator<SearchEnrollmentsQuery>
    {
        public SearchEnrollmentsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage("Search cannot exceed 100 characters.");

            RuleFor(x => x)
                .Must(x => !x.From.HasValue || !x.To.HasValue || x.From <= x.To)
                .WithMessage("'From' date must be less than or equal to 'To' date.");
        }
    }
}
