using ElearningPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Certificate : BaseEntity
    {
        public string StudentId { get; set; } = null!;

        public int CourseId { get; set; }

      
        public string CertificateNumber { get; set; } = null!;

       
        public string VerificationCode { get; set; } = null!;

     
        public string CertificateUrl { get; set; } = null!;

        public DateTime IssuedAt { get; set; }

      
        public bool IsRevoked { get; set; }

        public DateTime? RevokedAt { get; set; }

        public string? RevokedReason { get; set; }

        
        public int DownloadCount { get; set; }

        public DateTime? LastDownloadedAt { get; set; }

        public ApplicationUser Student { get; set; } = null!;

        public Course Course { get; set; } = null!;
    }
}
