using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending = 1,      // فى انتظار الدفع
        Succeeded = 2,    // نجح الدفع
        Failed = 3,       // فشل الدفع
        Cancelled = 4,    // ألغى العميل الدفع
        Refunded = 5      // تم استرجاع المبلغ
    }
}
