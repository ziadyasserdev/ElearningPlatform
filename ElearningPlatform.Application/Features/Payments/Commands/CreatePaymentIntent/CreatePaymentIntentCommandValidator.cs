using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Payments.Commands.CreatePaymentIntent
{
    public class CreatePaymentIntentValidator : AbstractValidator<CreatePaymentIntentCommand>
    {
        public CreatePaymentIntentValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage("Order Id is required.");
        }
    }
}
