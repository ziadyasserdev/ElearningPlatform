using ElearningPlatform.Application.Contracts.Payments;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElearningPlatform.Application.Features.Payments.Dtos;
namespace ElearningPlatform.Infrastructure.Payments
{
    public class StripePaymentService : IPaymentService
    {
        private readonly StripeOptions options;

        public StripePaymentService(IOptions<StripeOptions> options)
        {
            this.options = options.Value;

            StripeConfiguration.ApiKey = this.options.SecretKey;
        }

        public async Task<Application.Features.Payments.Dtos.PaymentIntentResult> CreatePaymentIntentAsync(
            decimal amount,
            string currency,
            CancellationToken cancellationToken = default)
        {
            var service = new PaymentIntentService();

            var paymentIntent = await service.CreateAsync(
                new PaymentIntentCreateOptions
                {
                    Amount = (long)(amount * 100),
                    Currency = currency.ToLower(),
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true
                    }
                },
                cancellationToken: cancellationToken);
            return new Application.Features.Payments.Dtos.PaymentIntentResult
            {
                PaymentIntentId = paymentIntent.Id,
                ClientSecret = paymentIntent.ClientSecret
            };

            
        }

      
    }
}
