using ElearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Payments.Dtos
{
    public class VerifyPaymentDto
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public PaymentStatus Status { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime? PaidAt { get; set; }

        public string? FailureReason { get; set; }
    }
}
