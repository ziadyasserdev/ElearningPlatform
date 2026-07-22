using ElearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Payments.Dtos
{
    public class PaymentDetailsDto
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public string Provider { get; set; }

        public string Status { get; set; }

        public string? TransactionId { get; set; }

        public string? PaymentIntentId { get; set; }

        public DateTime? PaidAt { get; set; }

        public string? FailureReason { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
