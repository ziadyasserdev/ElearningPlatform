using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Queries.GetCourseReviewSummary
{
    public class GetCourseReviewSummaryQueryValidator
        : AbstractValidator<GetCourseReviewSummaryQuery>
    {
        public GetCourseReviewSummaryQueryValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("Course Id must be greater than 0.");
        }
    }
}
