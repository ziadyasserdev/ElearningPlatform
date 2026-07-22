using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Payments.Commands.RetryPayment
{
    public class RetryPaymentValidator
          : AbstractValidator<RetryPaymentCommand>
    {
        public RetryPaymentValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);
        }
    }
}
