using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Commands.CreateCoupon
{
    public class CreateCouponCommandHandler
        : IRequestHandler<CreateCouponCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateCouponCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            CreateCouponCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");

            if (!currentUserService.IsInRole("Admin"))
                return Result<int>.Failure(
                    ResultStatus.Forbidden,
                    "Only administrators can create coupons.");

            var code = request.Code.Trim().ToUpper();

            var exists = await unitOfWork.Coupons.Query()
                .AnyAsync(x =>
                    x.Code == code &&
                    !x.IsDeleted,
                    cancellationToken);

            if (exists)
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Coupon code already exists.");

            var coupon = new Coupon
            {
                Code = code,
                Description = request.Description.Trim(),
                Type = request.Type,
                Value = request.Value,
                MinimumOrderAmount = request.MinimumOrderAmount,
                MaximumDiscountAmount = request.MaximumDiscountAmount,
                UsageLimit = request.UsageLimit,
                UsedCount = 0,
                IsActive = true,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName
            };

            await unitOfWork.Coupons.AddAsync(coupon);

            await unitOfWork.SaveAsync();

            return Result<int>.Success(coupon.Id);
        }
    }
}
