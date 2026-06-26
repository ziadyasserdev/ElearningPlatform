using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Dtos
{
    public class CourseAdminDto
    {
        public int Id { get; set; }

        // Basic info
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }

        // Pricing
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public DateTime? DiscountEndDate { get; set; }

        // Status control
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsDeleted { get; set; }

        // Stats
        public int TotalStudents { get; set; }
        public int TotalLessons { get; set; }
        public int TotalDurationInMinutes { get; set; }
        public int TotalReviews { get; set; }
        public decimal AverageRating { get; set; }

        // Relations
        public string InstructorName { get; set; }
        public string CategoryName { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}
