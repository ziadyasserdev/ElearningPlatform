using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Payments;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Payments.Commands.RetryPayment
{
    public class RetryPaymentCommandHandler
      : IRequestHandler<RetryPaymentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IPaymentService paymentService;

        public RetryPaymentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IPaymentService paymentService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.paymentService = paymentService;
        }

        public async Task<Result<string>> Handle(
            RetryPaymentCommand request,
            CancellationToken cancellationToken)
        {
            var payment = await unitOfWork.Payments.Query()
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x =>
                    x.OrderId == request.OrderId,
                    cancellationToken);

            if (payment is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Payment not found.");
            }

            if (payment.Order.StudentId != currentUserService.UserId)
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You are not allowed to retry this payment.");
            }

            if (payment.Order.Status == OrderStatus.Paid)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "This order has already been paid.");
            }

            if (payment.Status == PaymentStatus.Paid)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Payment already completed.");
            }

            var paymentIntent = await paymentService.CreatePaymentIntentAsync(
                payment.Amount,
                payment.Currency,
                cancellationToken);

            payment.PaymentIntentId = paymentIntent.PaymentIntentId;
            payment.ClientSecret = paymentIntent.ClientSecret;
            payment.TransactionId = null;
            payment.FailureReason = null;
            payment.Status = PaymentStatus.Pending;
            payment.UpdatedAt = DateTime.Now;

            payment.Order.Status = OrderStatus.Pending;
            payment.Order.UpdatedAt = DateTime.Now;

            await unitOfWork.SaveAsync();

            return Result<string>.Success(paymentIntent.ClientSecret);
        }
    }
}
