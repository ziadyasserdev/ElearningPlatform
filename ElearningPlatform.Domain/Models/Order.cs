using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Order : BaseEntity
    {
        public string StudentId { get; set; } = null!;

        public string OrderNumber { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }
        public DateTime? PaidAt { get; set; }

        public int? CouponId { get; set; }

        public ApplicationUser Student { get; set; } = null!;

        public Coupon? Coupon { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Payment? Payment { get; set; }
    }
}
