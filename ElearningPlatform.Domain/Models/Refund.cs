using ElearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Refund : BaseEntity
    {
        public int PaymentId { get; set; }

        public decimal Amount { get; set; }

        public string Reason { get; set; } = string.Empty;

        public RefundStatus Status { get; set; }

        public DateTime? RefundedAt { get; set; }

        public Payment Payment { get; set; } = null!;
    }
}
