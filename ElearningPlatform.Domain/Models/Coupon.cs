using ElearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Coupon : BaseEntity
    {
        public string Code { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public CouponType Type { get; set; }

        public decimal Value { get; set; }

        public decimal? MinimumOrderAmount { get; set; }

        public decimal? MaximumDiscountAmount { get; set; }

        public int UsageLimit { get; set; }

        public int UsedCount { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
