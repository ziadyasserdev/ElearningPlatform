using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Payments;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Payments.Commands.CreatePaymentIntent
{
    public class CreatePaymentIntentCommandHandler
       : IRequestHandler<CreatePaymentIntentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IPaymentService paymentService;

        public CreatePaymentIntentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IPaymentService paymentService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.paymentService = paymentService;
        }

        public async Task<Result<string>> Handle(
            CreatePaymentIntentCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");

            var userId = currentUserService.UserId;

            var order = await unitOfWork.Orders.Query()
                .Include(x => x.Payment)
                .FirstOrDefaultAsync(x =>
                    x.Id == request.OrderId &&
                    x.StudentId == userId,
                    cancellationToken);

            if (order is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Order not found.");

            if (order.Status != OrderStatus.Pending)
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Order cannot be paid.");

            if (order.Payment != null &&
                order.Payment.Status == PaymentStatus.Paid)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Order has already been paid.");
            }

            var paymentIntent = await paymentService.CreatePaymentIntentAsync(
                order.TotalAmount,
                "usd",
                cancellationToken);

            if (order.Payment is null)
            {
                var payment = new Payment
                {
                    OrderId = order.Id,
                    Amount = order.TotalAmount,
                    Currency = "usd",
                    Provider = PaymentProvider.Stripe,
                    Status = PaymentStatus.Pending,
                    PaymentIntentId = paymentIntent.PaymentIntentId,
                    ClientSecret = paymentIntent.ClientSecret,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUserService.UserName
                };

                await unitOfWork.Payments.AddAsync(payment);
            }
            else
            {
                order.Payment.PaymentIntentId = paymentIntent.PaymentIntentId;
                order.Payment.ClientSecret = paymentIntent.ClientSecret;
                order.Payment.Status = PaymentStatus.Pending;
                order.Payment.UpdatedAt = DateTime.Now;
                order.Payment.UpdatedBy = currentUserService.UserName;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success(paymentIntent.ClientSecret);
        }
    }
}
