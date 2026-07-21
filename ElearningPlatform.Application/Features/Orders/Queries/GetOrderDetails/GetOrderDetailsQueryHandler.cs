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

namespace ElearningPlatform.Application.Features.Orders.Queries.GetOrderDetails
{
    public class GetOrderDetailsQueryHandler
       : IRequestHandler<GetOrderDetailsQuery, Result<OrderDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetOrderDetailsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<OrderDetailsDto>> Handle(
            GetOrderDetailsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<OrderDetailsDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");

            var userId = currentUserService.UserId;

            var order = await unitOfWork.Orders.Query()
                .AsNoTracking()
                .Include(x => x.Coupon)
                .Include(x => x.OrderItems)
                .Include(x => x.Payment)
                .FirstOrDefaultAsync(x =>
                    x.Id == request.OrderId &&
                    x.StudentId == userId,
                    cancellationToken);

            if (order == null)
                return Result<OrderDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Order not found");

            var dto = new OrderDetailsDto
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                SubTotal = order.SubTotal,
                DiscountAmount = order.DiscountAmount,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                PaidAt = order.PaidAt,
                CouponCode = order.Coupon?.Code,

                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    CourseId = i.CourseId,
                    CourseTitle = i.CourseTitle,
                    Price = i.Price,
                    Discount = i.Discount,
                    FinalPrice = i.FinalPrice
                }).ToList(),

                Payment = order.Payment == null
                    ? null
                    : new PaymentDto
                    {
                        Provider = order.Payment.Provider.ToString(),
                        Status = order.Payment.Status.ToString(),
                        Amount = order.Payment.Amount,
                        Currency = order.Payment.Currency,
                        TransactionId = order.Payment.TransactionId,
                        PaidAt = order.Payment.PaidAt
                    }
            };

            return Result<OrderDetailsDto>.Success(dto);
        }
    }
}
