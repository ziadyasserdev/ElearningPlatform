using ElearningPlatform.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Commands.UpdateCoupon
{
    public class UpdateCouponValidator : AbstractValidator<UpdateCouponCommand>
    {
        public UpdateCouponValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

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
                .When(x => x.Type == CouponType.Percentage);

            RuleFor(x => x.MaximumDiscountAmount)
                .Null()
                .When(x => x.Type == CouponType.FixedAmount);
        }
    }
}
