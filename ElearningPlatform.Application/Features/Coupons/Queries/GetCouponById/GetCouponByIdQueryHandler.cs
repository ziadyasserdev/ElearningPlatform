using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Coupons.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Queries.GetCouponById
{
    public class GetCouponByIdQueryHandler
       : IRequestHandler<GetCouponByIdQuery, Result<CouponDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCouponByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<CouponDetailsDto>> Handle(
            GetCouponByIdQuery request,
            CancellationToken cancellationToken)
        {
            var coupon = await unitOfWork.Coupons.Query()
                .AsNoTracking()
                .Where(x => x.Id == request.Id && !x.IsDeleted)
                .Select(x => new CouponDetailsDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    Type = x.Type,
                    Value = x.Value,
                    MinimumOrderAmount = x.MinimumOrderAmount,
                    MaximumDiscountAmount = x.MaximumDiscountAmount,
                    UsageLimit = x.UsageLimit,
                    UsedCount = x.UsedCount,
                    IsActive = x.IsActive,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (coupon == null)
                return Result<CouponDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Coupon not found.");

            return Result<CouponDetailsDto>.Success(coupon);
        }
    }
}
