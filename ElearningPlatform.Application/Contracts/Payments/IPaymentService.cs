using ElearningPlatform.Application.Features.Payments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Contracts.Payments
{
    public interface IPaymentService
    {
     public   Task<PaymentIntentResult> CreatePaymentIntentAsync(
               decimal amount,
               string currency,
               CancellationToken cancellationToken = default);
    }
}
