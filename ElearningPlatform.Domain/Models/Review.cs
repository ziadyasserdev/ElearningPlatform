using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Review : BaseEntity
    {
        public string StudentId { get; set; } = null!;

        public int CourseId { get; set; }

       
        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

      
        public ReviewStatus Status { get; set; }

        public string? RejectionReason { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public string? ApprovedBy { get; set; }

        public DateTime? EditedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }    

        public string? ReviewedBy { get; set; }      
        public ApplicationUser Student { get; set; } = null!;

        public Course Course { get; set; } = null!;
    }
}
