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

namespace ElearningPlatform.Application.Features.Carts.Commands.RemoveCoupon
{
    public class RemoveCouponCommandHandler
        : IRequestHandler<RemoveCouponCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RemoveCouponCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            RemoveCouponCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");

            var userId = currentUserService.UserId;

            var cart = await unitOfWork.Carts.Query()
                .FirstOrDefaultAsync(x => x.StudentId == userId, cancellationToken);

            if (cart is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Cart not found.");

            if (!cart.CouponId.HasValue)
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "No coupon has been applied.");

            cart.CouponId = null;

            cart.UpdatedAt = DateTime.Now;
            cart.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Coupon removed successfully.");
        }
    }
}
