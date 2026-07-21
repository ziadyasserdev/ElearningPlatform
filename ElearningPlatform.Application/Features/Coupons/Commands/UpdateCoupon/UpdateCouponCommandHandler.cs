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

namespace ElearningPlatform.Application.Features.Coupons.Commands.UpdateCoupon
{
    public class UpdateCouponCommandHandler
        : IRequestHandler<UpdateCouponCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateCouponCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            UpdateCouponCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");

            if (!currentUserService.IsInRole("Admin"))
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "Only administrators can update coupons.");

            var coupon = await unitOfWork.Coupons.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted,
                    cancellationToken);

            if (coupon == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Coupon not found.");

            var code = request.Code.Trim().ToUpper();

            var exists = await unitOfWork.Coupons.Query()
                .AnyAsync(x =>
                    x.Id != request.Id &&
                    x.Code == code &&
                    !x.IsDeleted,
                    cancellationToken);

            if (exists)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Coupon code already exists.");

            coupon.Code = code;
            coupon.Description = request.Description.Trim();
            coupon.Type = request.Type;
            coupon.Value = request.Value;
            coupon.MinimumOrderAmount = request.MinimumOrderAmount;
            coupon.MaximumDiscountAmount = request.MaximumDiscountAmount;
            coupon.UsageLimit = request.UsageLimit;
            coupon.StartDate = request.StartDate;
            coupon.EndDate = request.EndDate;
            coupon.IsActive = request.IsActive;

            coupon.UpdatedAt = DateTime.UtcNow;
            coupon.UpdatedBy = currentUserService.UserName;

            unitOfWork.Coupons.Update(coupon);

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Coupon updated successfully.");
        }
    }
}
