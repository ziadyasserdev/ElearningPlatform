using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Orders.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Orders.Queries.GetOrderDetails
{
    public class GetOrderDetailsQuery : IRequest<Result<OrderDetailsDto>>
    {
        public int OrderId { get; set; }

        public GetOrderDetailsQuery(int orderId)
        {
            OrderId = orderId;
        }
    }
}
