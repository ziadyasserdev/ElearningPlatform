using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Payments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Payments.Queries.VerifyPayment
{
    public class VerifyPaymentQuery
         : IRequest<Result<VerifyPaymentDto>>
    {
        public string PaymentIntentId { get; set; } = string.Empty;

        public VerifyPaymentQuery(string paymentIntentId)
        {
            PaymentIntentId = paymentIntentId;
        }
    }
}
