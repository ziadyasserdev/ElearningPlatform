using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
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

namespace ElearningPlatform.Application.Features.Orders.Commands.Checkout
{
    public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CheckoutCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            CheckoutCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");

            var userId = currentUserService.UserId;

            var cart = await unitOfWork.Carts.Query()
                .Include(x => x.Coupon)
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(x => x.StudentId == userId, cancellationToken);

            if (cart is null)
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Cart not found.");

            if (!cart.CartItems.Any())
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "Your cart is empty.");

            decimal subTotal = 0;

            foreach (var item in cart.CartItems)
            {
                if (!item.Course.IsActive ||
                    item.Course.IsDeleted ||
                    item.Course.Status != CourseStatus.Published)
                {
                    return Result<int>.Failure(
                        ResultStatus.Failure,
                        $"Course '{item.Course.Title}' is no longer available.");
                }

                subTotal += item.Course.DiscountPrice ?? item.Course.Price;
            }

            decimal discountAmount = 0;

            if (cart.Coupon != null)
            {
                var coupon = cart.Coupon;

                var now = DateTime.UtcNow;

                if (!coupon.IsActive)
                    return Result<int>.Failure(
                        ResultStatus.Failure,
                        "Coupon is inactive.");

                if (coupon.StartDate > now)
                    return Result<int>.Failure(
                        ResultStatus.Failure,
                        "Coupon is not available yet.");

                if (coupon.EndDate < now)
                    return Result<int>.Failure(
                        ResultStatus.Failure,
                        "Coupon has expired.");

                if (coupon.UsedCount >= coupon.UsageLimit)
                    return Result<int>.Failure(
                        ResultStatus.Failure,
                        "Coupon usage limit has been reached.");

                var alreadyUsed = await unitOfWork.CouponUsages.Query()
                    .AnyAsync(x =>
                        x.CouponId == coupon.Id &&
                        x.StudentId == userId,
                        cancellationToken);

                if (alreadyUsed)
                    return Result<int>.Failure(
                        ResultStatus.Failure,
                        "You have already used this coupon.");

                if (coupon.MinimumOrderAmount.HasValue &&
                    subTotal < coupon.MinimumOrderAmount.Value)
                {
                    return Result<int>.Failure(
                        ResultStatus.Failure,
                        $"Minimum order amount is {coupon.MinimumOrderAmount.Value}.");
                }

                if (coupon.Type == CouponType.Percentage)
                {
                    discountAmount = subTotal * coupon.Value / 100;

                    if (coupon.MaximumDiscountAmount.HasValue &&
                        discountAmount > coupon.MaximumDiscountAmount.Value)
                    {
                        discountAmount = coupon.MaximumDiscountAmount.Value;
                    }
                }
                else
                {
                    discountAmount = coupon.Value;
                }

                if (discountAmount > subTotal)
                    discountAmount = subTotal;
            }

            var order = new Order
            {
                StudentId = userId,
                CouponId = cart.CouponId,
                OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}",
                Status = OrderStatus.Pending,
                SubTotal = subTotal,
                DiscountAmount = discountAmount,
                TotalAmount = subTotal - discountAmount,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserService.UserName
            };

            await unitOfWork.Orders.AddAsync(order);

            foreach (var item in cart.CartItems)
            {
                var price = item.Course.Price;
                var finalPrice = item.Course.DiscountPrice ?? price;

                var orderItem = new OrderItem
                {
                    Order = order,
                    CourseId = item.CourseId,
                    Price = price,
                    Discount = price - finalPrice,
                    FinalPrice = finalPrice,
                    CreatedAt = DateTime.Now,
                    CreatedBy = currentUserService.UserName
                };

                await unitOfWork.OrderItems.AddAsync(orderItem);
            }

            await unitOfWork.SaveAsync();

            return Result<int>.Success(order.Id);
        }
    }
}
