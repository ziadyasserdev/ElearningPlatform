using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ElearningPlatform.Domain.Models
{

    public class Course : BaseEntity
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public CourseLevel Level { get; set; }
        public string Language { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int InstructorId { get; set; }
        public int CategoryId { get; set; }
        public CourseStatus Status { get; set; }
        public int TotalDurationInMinutes { get; set; }
        public int TotalLessons { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalStudents { get; set; }
        public DateTime? PublishedAt { get; set; }
        public int TotalReviews { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        public bool IsFree { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public InstructorProfile Instructor { get; set; }
        public Category Category { get; set; }
        public ICollection<Section> Sections { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Exam> Exams { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Certificate> Certificates { get; set; } = new HashSet<Certificate>();
    }

}
