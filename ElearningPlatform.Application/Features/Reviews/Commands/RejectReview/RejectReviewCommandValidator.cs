using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Commands.RejectReview
{
    public class RejectReviewCommandValidator
       : AbstractValidator<RejectReviewCommand>
    {
        public RejectReviewCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Review Id must be greater than 0.");

            RuleFor(x => x.RejectionReason)
                .NotEmpty()
                .WithMessage("Rejection reason is required.")
                .MaximumLength(500)
                .WithMessage("Rejection reason cannot exceed 500 characters.");
        }
    }
}
