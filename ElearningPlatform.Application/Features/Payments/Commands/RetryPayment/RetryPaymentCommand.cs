using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Payments.Commands.RetryPayment
{
    public class RetryPaymentCommand : IRequest<Result<string>>
    {
        public int OrderId { get; set; }
    }
}
