using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Carts.Dtos
{
    public class CartResponse
    {
        public int TotalItems { get; set; }

      
        public decimal SubTotal { get; set; }

      
        public decimal CourseDiscount { get; set; }

      
        public decimal CouponDiscount { get; set; }

        
        public decimal Total { get; set; }

        public List<CartItemDto> Items { get; set; } = new();
    }
}
