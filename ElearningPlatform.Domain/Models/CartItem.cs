using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class CartItem : BaseEntity
    {
        public int CartId { get; set; }

        public int CourseId { get; set; }

       
        public DateTime AddedAt { get; set; }

        public Cart Cart { get; set; }

        public Course Course { get; set; }
    }
}
