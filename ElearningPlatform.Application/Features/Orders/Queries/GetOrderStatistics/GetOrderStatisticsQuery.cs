using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Orders.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Orders.Queries.GetOrderStatistics
{
    public class GetOrderStatisticsQuery
       : IRequest<Result<OrderStatisticsDto>>
    {
    }
}
