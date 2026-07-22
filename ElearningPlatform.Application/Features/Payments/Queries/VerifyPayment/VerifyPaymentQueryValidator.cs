using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Payments.Queries.VerifyPayment
{
    public class VerifyPaymentValidator
      : AbstractValidator<VerifyPaymentQuery>
    {
        public VerifyPaymentValidator()
        {
            RuleFor(x => x.PaymentIntentId)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
