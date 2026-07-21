using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Carts.Commands.ApplyCoupon
{
    public class ApplyCouponCommandHandler
      : IRequestHandler<ApplyCouponCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ApplyCouponCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            ApplyCouponCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");

            var userId = currentUserService.UserId;

            var cart = await unitOfWork.Carts.Query()
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(x => x.StudentId == userId, cancellationToken);

            if (cart is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Cart not found.");

            if (!cart.CartItems.Any())
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Your cart is empty.");

            var coupon = await unitOfWork.Coupons.Query()
                .FirstOrDefaultAsync(x =>
                    x.Code == request.Code &&
                    !x.IsDeleted,
                    cancellationToken);

            if (coupon is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Coupon not found.");

            if (!coupon.IsActive)
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Coupon is inactive.");

            var now = DateTime.UtcNow;

            if (coupon.StartDate > now)
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Coupon is not available yet.");

            if (coupon.EndDate < now)
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Coupon has expired.");

            if (coupon.UsedCount >= coupon.UsageLimit)
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Coupon usage limit has been reached.");

            var alreadyUsed = await unitOfWork.CouponUsages.Query()
                .AnyAsync(x =>
                    x.CouponId == coupon.Id &&
                    x.StudentId == userId,
                    cancellationToken);

            if (alreadyUsed)
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "You have already used this coupon.");

            var subTotal = cart.CartItems.Sum(x =>
                x.Course.DiscountPrice ?? x.Course.Price);

            if (coupon.MinimumOrderAmount.HasValue &&
                subTotal < coupon.MinimumOrderAmount.Value)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Minimum order amount is {coupon.MinimumOrderAmount.Value}.");
            }

            cart.CouponId = coupon.Id;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Coupon applied successfully.");
        }
    }

}
