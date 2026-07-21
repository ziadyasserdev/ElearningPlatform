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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly ICurrentUserService _currentUser;

        public CreatePaymentIntentCommandHandler(
            IUnitOfWork unitOfWork,
            IPaymentService paymentService,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _currentUser = currentUser;
        }


        public async Task<Result<string>> Handle(
            CreatePaymentIntentCommand request,
            CancellationToken cancellationToken)
        {


            var userId = _currentUser.UserId;



            var order = await _unitOfWork.Orders
                .GetByIdAsync(request.OrderId);


            if (order == null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Order not found");
            }



            if (order.StudentId != userId)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "You cannot pay for this order");
            }



            if (order.Status != OrderStatus.Pending)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Only pending orders can be paid");
            }




            var existingPayment = await _unitOfWork.Payments
                .Query()
                .FirstOrDefaultAsync(
                    x => x.OrderId == order.Id,
                    cancellationToken);


            if (existingPayment != null)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Payment already created");
            }





            var paymentIntent = await _paymentService
                .CreatePaymentIntentAsync(
                    order.TotalAmount,
                    "usd",
                    cancellationToken);





            var payment = new Payment
            {
                OrderId = order.Id,

                PaymentIntentId = paymentIntent.PaymentIntentId,

                ClientSecret = paymentIntent.ClientSecret,

                Amount = order.TotalAmount,

                Status = PaymentStatus.Pending,

                CreatedAt = DateTime.Now
            };


            await _unitOfWork.Payments.AddAsync(payment);




            await _unitOfWork.SaveAsync();





            return Result<string>.Success(
                paymentIntent.ClientSecret,
                "Payment intent created successfully");
        }
    }
}
