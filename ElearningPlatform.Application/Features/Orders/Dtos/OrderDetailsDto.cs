using ElearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Orders.Dtos
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? PaidAt { get; set; }

        public string? CouponCode { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();

        public PaymentDto? Payment { get; set; }
    }
}
