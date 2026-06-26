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

        public string CertificateUrl { get; set; } = string.Empty;

        public string VerificationCode { get; set; } = string.Empty;

        public DateTime IssuedAt { get; set; }

        public bool IsValid { get; set; } = true;

        public ApplicationUser Student { get; set; } = null!;

        public Course Course { get; set; } = null!;
    }
}
