using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandValidator
       : AbstractValidator<DeleteReviewCommand>
    {
        public DeleteReviewCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Review Id must be greater than 0.");
        }
    }
}
