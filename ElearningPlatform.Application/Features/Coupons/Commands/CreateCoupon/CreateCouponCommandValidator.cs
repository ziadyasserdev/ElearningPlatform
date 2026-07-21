using ElearningPlatform.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Commands.CreateCoupon
{
    public class CreateCouponValidator : AbstractValidator<CreateCouponCommand>
    {
        public CreateCouponValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x.Value)
                .GreaterThan(0);

            RuleFor(x => x.UsageLimit)
                .GreaterThan(0);

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate);

            RuleFor(x => x.Value)
                .LessThanOrEqualTo(100)
                .When(x => x.Type == CouponType.Percentage)
                .WithMessage("Percentage coupon cannot exceed 100%.");

            RuleFor(x => x.MaximumDiscountAmount)
                .Null()
                .When(x => x.Type == CouponType.FixedAmount)
                .WithMessage("Maximum discount is only valid for percentage coupons.");
        }
    }
}
