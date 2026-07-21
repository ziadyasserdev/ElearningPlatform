using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.GetGradedSubmissions
{
    public class GetGradedSubmissionsQueryValidator
       : AbstractValidator<GetGradedSubmissionsQuery>
    {
        public GetGradedSubmissionsQueryValidator()
        {
            RuleFor(x => x.AssignmentId)
                .GreaterThan(0)
                .WithMessage("Assignment Id must be greater than zero.");

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage("Search cannot exceed 100 characters.");

            RuleFor(x => x.MinScore)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinScore.HasValue)
                .WithMessage("Minimum score cannot be negative.");

            RuleFor(x => x.MaxScore)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MaxScore.HasValue)
                .WithMessage("Maximum score cannot be negative.");

            RuleFor(x => x)
                .Must(x =>
                    !x.MinScore.HasValue ||
                    !x.MaxScore.HasValue ||
                    x.MinScore <= x.MaxScore)
                .WithMessage("Minimum score must be less than or equal to maximum score.");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than zero.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");
        }
    }
}
