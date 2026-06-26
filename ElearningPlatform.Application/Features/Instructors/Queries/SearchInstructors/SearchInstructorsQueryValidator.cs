using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.SearchInstructors
{
    public class SearchInstructorsQueryValidator : AbstractValidator<SearchInstructorsQuery>
    {
        public SearchInstructorsQueryValidator()
        {
            RuleFor(x => x.MinRating)
                .InclusiveBetween(0, 5)
                .When(x => x.MinRating.HasValue);

            RuleFor(x => x.Name)
                .MaximumLength(100);

            RuleFor(x => x.Specialization)
                .MaximumLength(100);
        }
    }
}
