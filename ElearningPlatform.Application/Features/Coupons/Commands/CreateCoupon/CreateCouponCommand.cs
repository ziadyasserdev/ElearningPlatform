using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Commands.CreateCoupon
{
    public class CreateCouponCommand : IRequest<Result<int>>
    {
        public string Code { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public CouponType Type { get; set; }

        public decimal Value { get; set; }

        public decimal? MinimumOrderAmount { get; set; }

        public decimal? MaximumDiscountAmount { get; set; }

        public int UsageLimit { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
