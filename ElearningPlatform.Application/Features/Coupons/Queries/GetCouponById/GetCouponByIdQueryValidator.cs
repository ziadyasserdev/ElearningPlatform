using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Queries.GetCouponById
{
    public class GetCouponByIdValidator
       : AbstractValidator<GetCouponByIdQuery>
    {
        public GetCouponByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
