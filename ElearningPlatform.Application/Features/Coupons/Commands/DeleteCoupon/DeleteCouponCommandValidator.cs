using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Commands.DeleteCoupon
{
    public class DeleteCouponValidator
       : AbstractValidator<DeleteCouponCommand>
    {
        public DeleteCouponValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
