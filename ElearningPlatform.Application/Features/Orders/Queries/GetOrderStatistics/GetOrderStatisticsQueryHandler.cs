using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Orders.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Orders.Queries.GetOrderStatistics
{
    public class GetOrderStatisticsQueryHandler
      : IRequestHandler<GetOrderStatisticsQuery, Result<OrderStatisticsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetOrderStatisticsQueryHandler(
            IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<OrderStatisticsDto>> Handle(
            GetOrderStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<OrderStatisticsDto>.Failure(
                    ResultStatus.Unauthorized,
                    "You are not authorized to view order statistics.");

            if(!currentUserService.IsInRole("Admin"))
                return Result<OrderStatisticsDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not allowed to view order statistics.");


            var orders = unitOfWork.Orders.Query()
                .AsNoTracking();

            var statistics = new OrderStatisticsDto
            {
                TotalOrders = await orders.CountAsync(cancellationToken),

                PendingOrders = await orders.CountAsync(
                    x => x.Status == OrderStatus.Pending,
                    cancellationToken),

                ProcessingOrders = await orders.CountAsync(
                    x => x.Status == OrderStatus.Processing,
                    cancellationToken),

                PaidOrders = await orders.CountAsync(
                    x => x.Status == OrderStatus.Paid,
                    cancellationToken),

                FailedOrders = await orders.CountAsync(
                    x => x.Status == OrderStatus.Failed,
                    cancellationToken),

                CancelledOrders = await orders.CountAsync(
                    x => x.Status == OrderStatus.Cancelled,
                    cancellationToken),

                RefundedOrders = await orders.CountAsync(
                    x => x.Status == OrderStatus.Refunded,
                    cancellationToken),

                TotalRevenue = await orders
                    .Where(x => x.Status == OrderStatus.Paid)
                    .SumAsync(x => x.TotalAmount, cancellationToken)
            };

            return Result<OrderStatisticsDto>.Success(statistics);
        }
    }
}
