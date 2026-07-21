using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Contracts.Payments
{
    public interface IStripeWebhookService
    {
        Task HandleWebhookAsync(
            string json,
            string stripeSignature,
            CancellationToken cancellationToken = default);
    }
}
