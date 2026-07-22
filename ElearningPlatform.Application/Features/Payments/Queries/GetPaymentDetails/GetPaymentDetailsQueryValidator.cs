using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Payments.Queries.GetPaymentDetails
{
    public class GetPaymentDetailsValidator
        : AbstractValidator<GetPaymentDetailsQuery>
    {
        public GetPaymentDetailsValidator()
        {
            RuleFor(x => x.PaymentId)
                .GreaterThan(0);
        }
    }
}
