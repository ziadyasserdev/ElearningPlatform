using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Orders.Dtos
{
    public class OrderItemDto
    {
        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public decimal FinalPrice { get; set; }
    }
}
