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

namespace ElearningPlatform.Application.Features.Payments.Queries.GetPaymentDetails
{
    public class GetPaymentDetailsQueryHandler
        : IRequestHandler<GetPaymentDetailsQuery, Result<PaymentDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetPaymentDetailsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaymentDetailsDto>> Handle(
            GetPaymentDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var payment = await unitOfWork.Payments.Query()
                .Include(x => x.Order)
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.PaymentId,
                    cancellationToken);

            if (payment is null)
            {
                return Result<PaymentDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Payment not found.");
            }

          
            if (!currentUserService.IsInRole("Admin") &&
                payment.Order.StudentId != currentUserService.UserId)
            {
                return Result<PaymentDetailsDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not allowed to view this payment.");
            }

            var dto = new PaymentDetailsDto
            {
                PaymentId = payment.Id,
                OrderId = payment.OrderId,
                OrderNumber = payment.Order.OrderNumber,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Provider = payment.Provider.ToString(),
                Status = payment.Status.ToString(),
                TransactionId = payment.TransactionId,
                PaymentIntentId = payment.PaymentIntentId,
                PaidAt = payment.PaidAt,
                FailureReason = payment.FailureReason,
                CreatedAt = payment.CreatedAt
            };

            return Result<PaymentDetailsDto>.Success(dto);
        }
    }
}
