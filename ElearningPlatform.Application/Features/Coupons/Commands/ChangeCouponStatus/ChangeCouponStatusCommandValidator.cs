using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Commands.ChangeCouponStatus
{
    public class ChangeCouponStatusValidator
         : AbstractValidator<ChangeCouponStatusCommand>
    {
        public ChangeCouponStatusValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
