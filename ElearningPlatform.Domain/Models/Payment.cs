using ElearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Payment : BaseEntity
    {
        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "USD";

        public PaymentProvider Provider { get; set; }

        public string? TransactionId { get; set; }

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime? PaidAt { get; set; }

        public string? FailureReason { get; set; }

        public Order Order { get; set; } = null!;

        public Refund? Refund { get; set; }
    }
}
