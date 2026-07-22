using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Commands.DeleteReviewByAdmin
{
    public class DeleteReviewByAdminCommandValidator
       : AbstractValidator<DeleteReviewByAdminCommand>
    {
        public DeleteReviewByAdminCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Review Id must be greater than 0.");
        }
    }
}
