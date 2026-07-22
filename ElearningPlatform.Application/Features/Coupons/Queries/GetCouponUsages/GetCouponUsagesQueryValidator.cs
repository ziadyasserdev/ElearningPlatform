using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Queries.GetCouponUsages
{
    public class GetCouponUsagesValidator
      : AbstractValidator<GetCouponUsagesQuery>
    {
        public GetCouponUsagesValidator()
        {
            RuleFor(x => x.CouponId)
                .GreaterThan(0);

            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);
        }
    }
}
