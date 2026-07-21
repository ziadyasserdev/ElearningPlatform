using ElearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Orders.Dtos
{
    public class PaymentDto
    {
        public string Provider { get; set; }

        public string Status { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public string? TransactionId { get; set; }

        public DateTime? PaidAt { get; set; }
    }
}
