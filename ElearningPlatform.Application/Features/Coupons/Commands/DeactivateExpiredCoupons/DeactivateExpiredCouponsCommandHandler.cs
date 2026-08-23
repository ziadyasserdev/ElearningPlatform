using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Commands.DeactivateExpiredCoupons
{
    public class DeactivateExpiredCouponsCommandHandler : IRequestHandler<DeactivateExpiredCouponsCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeactivateExpiredCouponsCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeactivateExpiredCouponsCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var coupons = await unitOfWork.Coupons.Query()
                .Where(c => !c.IsDeleted && c.IsActive && c.EndDate <= now)
                .ToListAsync(cancellationToken);
           foreach(var coupon in  coupons)
            {
                coupon.IsActive = false;
                coupon.UpdatedAt = now;
                coupon.UpdatedBy = "System";
            
            }
           await unitOfWork.SaveAsync();
            return Result<int>.Success(coupons.Count);
        }
    }
}
