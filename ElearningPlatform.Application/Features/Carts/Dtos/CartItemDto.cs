using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Carts.Dtos
{
    public class CartItemDto
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? ThumbnailUrl { get; set; }

        public string InstructorName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public decimal FinalPrice { get; set; }
    }
}
