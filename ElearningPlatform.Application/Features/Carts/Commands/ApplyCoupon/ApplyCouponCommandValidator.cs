using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Carts.Commands.ApplyCoupon
{
    public class ApplyCouponValidator : AbstractValidator<ApplyCouponCommand>
    {
        public ApplyCouponValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Coupon code is required.")
                .MaximumLength(50)
                .WithMessage("Coupon code cannot exceed 50 characters.");

            RuleFor(x => x.Code)
                .Matches("^[A-Za-z0-9_-]+$")
                .WithMessage("Coupon code contains invalid characters.");
        }
    }
}
