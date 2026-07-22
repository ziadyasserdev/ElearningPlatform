using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Coupons.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Queries.GetCoupons
{
    public class GetCouponsQueryHandler
          : IRequestHandler<GetCouponsQuery, Result<PaginatedResult<CouponDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCouponsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<CouponDto>>> Handle(
            GetCouponsQuery request,
            CancellationToken cancellationToken)
        {
            var query = unitOfWork.Coupons.Query()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Code.Contains(request.Search) ||
                    x.Description.Contains(request.Search));
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(x =>
                    x.IsActive == request.IsActive.Value);
            }

            if (request.Expired.HasValue)
            {
                if (request.Expired.Value)
                {
                    query = query.Where(x =>
                        x.EndDate < DateTime.UtcNow);
                }
                else
                {
                    query = query.Where(x =>
                        x.EndDate >= DateTime.UtcNow);
                }
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var coupons = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CouponDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    Type = x.Type,
                    Value = x.Value,
                    UsageLimit = x.UsageLimit,
                    UsedCount = x.UsedCount,
                    IsActive = x.IsActive,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                })
                .ToListAsync(cancellationToken);

           return Result<PaginatedResult<CouponDto>>.Success(
                new PaginatedResult<CouponDto>(
                    coupons,
                    totalCount,
                    request.PageNumber,
                    request.PageSize));
        }
    }
}
