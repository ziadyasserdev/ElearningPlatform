using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Payments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Payments.Queries.GetPaymentDetails
{
    public class GetPaymentDetailsQuery
       : IRequest<Result<PaymentDetailsDto>>
    {
        public int PaymentId { get; set; }

        public GetPaymentDetailsQuery(int paymentId)
        {
            PaymentId = paymentId;
        }
    }
}
