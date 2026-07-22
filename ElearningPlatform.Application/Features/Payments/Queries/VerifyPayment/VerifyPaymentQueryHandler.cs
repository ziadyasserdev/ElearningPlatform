using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Payments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Payments.Queries.VerifyPayment
{
    public class VerifyPaymentQueryHandler
      : IRequestHandler<VerifyPaymentQuery, Result<VerifyPaymentDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public VerifyPaymentQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<VerifyPaymentDto>> Handle(
            VerifyPaymentQuery request,
            CancellationToken cancellationToken)
        {
            var payment = await unitOfWork.Payments.Query()
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x =>
                    x.PaymentIntentId == request.PaymentIntentId,
                    cancellationToken);

            if (payment == null)
            {
                return Result<VerifyPaymentDto>.Failure(
                    ResultStatus.NotFound,
                    "Payment not found.");
            }

            if (!currentUserService.IsInRole("Admin") &&
                payment.Order.StudentId != currentUserService.UserId)
            {
                return Result<VerifyPaymentDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not allowed to view this payment.");
            }

            var dto = new VerifyPaymentDto
            {
                PaymentId = payment.Id,
                OrderId = payment.OrderId,
                Status = payment.Status,
                OrderStatus = payment.Order.Status,
                PaidAt = payment.PaidAt,
                FailureReason = payment.FailureReason
            };

            return Result<VerifyPaymentDto>.Success(dto);
        }
    }
}
