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

namespace ElearningPlatform.Application.Features.Coupons.Commands.DeleteCoupon
{
    public class DeleteCouponCommandHandler
         : IRequestHandler<DeleteCouponCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteCouponCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            DeleteCouponCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");

            if (!currentUserService.IsInRole("Admin"))
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "Only administrators can delete coupons.");

            var coupon = await unitOfWork.Coupons.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted,
                    cancellationToken);

            if (coupon == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Coupon not found.");

            coupon.IsDeleted = true;
            coupon.IsActive = false;
            coupon.UpdatedAt = DateTime.Now;
            coupon.UpdatedBy = currentUserService.UserName;

          

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Coupon deleted successfully.");
        }
    }
}
