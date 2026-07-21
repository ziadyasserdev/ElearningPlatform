using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }

        public int CourseId { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public decimal FinalPrice { get; set; }

        public Order Order { get; set; } = null!;

        public Course Course { get; set; } = null!;
    }
}
