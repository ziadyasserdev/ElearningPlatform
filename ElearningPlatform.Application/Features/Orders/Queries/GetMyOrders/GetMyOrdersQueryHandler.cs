using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Orders.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Orders.Queries.GetMyOrders
{
    public class GetMyOrdersQueryHandler
        : IRequestHandler<GetMyOrdersQuery, Result<List<OrderDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyOrdersQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<OrderDto>>> Handle(
            GetMyOrdersQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<List<OrderDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");
            }

            var userId = currentUserService.UserId;

            var orders = await unitOfWork.Orders.Query()
                .AsNoTracking()
                .Where(x => x.StudentId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new OrderDto
                {
                    OrderId = x.Id,
                    OrderNumber = x.OrderNumber,
                    TotalAmount = x.TotalAmount,
                    DiscountAmount = x.DiscountAmount,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    PaidAt = x.PaidAt,
                    CoursesCount = x.OrderItems.Count
                })
                .ToListAsync(cancellationToken);

            return Result<List<OrderDto>>.Success(orders);
        }
    }
}
