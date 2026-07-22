using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Coupons.Dtos;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Queries.GetCouponUsages
{
    public class GetCouponUsagesQueryHandler
     : IRequestHandler<GetCouponUsagesQuery, Result<PaginatedResult<CouponUsageDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCouponUsagesQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<CouponUsageDto>>> Handle(
            GetCouponUsagesQuery request,
            CancellationToken cancellationToken)
        {
            var coupon = await unitOfWork.Coupons.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.CouponId &&
                    !x.IsDeleted,
                    cancellationToken);

            if (coupon is null)
            {
                return Result<PaginatedResult<CouponUsageDto>>.Failure(
                    ResultStatus.NotFound,
                    "Coupon not found");
            }

            var query = unitOfWork.CouponUsages.Query()
                .AsNoTracking()
                .Where(x => x.CouponId == request.CouponId)
                .Include(x => x.Student)
                .Include(x => x.Order)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.Student.FullName.Contains(search) ||
                    x.Student.Email!.Contains(search) ||
                    x.Order.OrderNumber.Contains(search));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var usages = await query
                .OrderByDescending(x => x.UsedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CouponUsageDto
                {
                    OrderId = x.OrderId,
                    OrderNumber = x.Order.OrderNumber,
                    StudentId = x.StudentId,
                    StudentName = x.Student.FullName,
                    StudentEmail = x.Student.Email!,
                    UsedAt = x.UsedAt
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<CouponUsageDto>(usages,request.PageNumber,request.PageSize,totalCount);
         

            return Result<PaginatedResult<CouponUsageDto>>.Success(result);
        }
    }
}
