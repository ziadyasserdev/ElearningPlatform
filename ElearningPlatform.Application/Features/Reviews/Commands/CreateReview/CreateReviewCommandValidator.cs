using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Commands.CreateReview
{

    public class CreateReviewCommandValidator
        : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5);

            RuleFor(x => x.Comment)
                .NotEmpty()
                .MaximumLength(1000);
        }
    }

}
