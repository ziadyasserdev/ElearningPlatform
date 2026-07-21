using ElearningPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class CouponUsage : BaseEntity
    {
        public int CouponId { get; set; }

        public string StudentId { get; set; } = null!;

        public int OrderId { get; set; }

        public DateTime UsedAt { get; set; }

        public Coupon Coupon { get; set; } = null!;

        public ApplicationUser Student { get; set; } = null!;

        public Order Order { get; set; } = null!;
    }
}
