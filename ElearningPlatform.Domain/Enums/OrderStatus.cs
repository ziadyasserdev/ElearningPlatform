using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Enums
{
    public enum OrderStatus
    {
        Pending = 1,      
        Processing = 2,   
        Paid = 3,        
        Failed = 4,      
        Cancelled = 5,   
        Refunded = 6      // تم استرجاع المبلغ
    }
}
