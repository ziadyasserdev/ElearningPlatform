using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorReviews
{
    public class GetInstructorReviewsQueryValidator
     : AbstractValidator<GetInstructorReviewsQuery>
    {
        public GetInstructorReviewsQueryValidator()
        {
            RuleFor(x => x.InstructorId)

               .NotEmpty().WithMessage("Instructor ID is required.");
        }
    }
}
