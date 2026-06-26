using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Identity
{
    public class ApplicationUser: IdentityUser
    {
      
        public string FullName { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;

       
        public bool IsActive { get; set; } = true;        
        public bool IsDeleted { get; set; } = false;      
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

     public Gender Gender { get; set; }
        public bool IsInstructor { get; set; } = false;

       
        public InstructorProfile? InstructorProfile { get; set; }

        public bool IsLocked { get; set; } = false;
        public DateTime? LockedAt { get; set; }
        public string? LockedByAdminId { get; set; }

        public ICollection<VideoProgress> VideoProgresses { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();
    }
}
