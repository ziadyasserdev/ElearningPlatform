using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommandHandler
     : IRequestHandler<CancelOrderCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CancelOrderCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            CancelOrderCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "You are not authorized");


            var order = await unitOfWork.Orders.Query()
                .Include(x => x.Payment)
                .FirstOrDefaultAsync(x =>
                    x.Id == request.OrderId,
                    cancellationToken);

            if (order == null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Order not found.");
            }

            if (order.StudentId != currentUserService.UserId)
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You are not allowed to cancel this order.");
            }
            if (order.Status == OrderStatus.Paid ||
                order.Status == OrderStatus.Processing)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "This order cannot be cancelled.");
            }

            if (order.Status == OrderStatus.Cancelled)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Order is already cancelled.");
            }

            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.Now;

            if (order.Payment != null)
            {
                order.Payment.Status = PaymentStatus.Cancelled;
                order.Payment.UpdatedAt = DateTime.Now;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Order cancelled successfully.");
        }
    }
}
