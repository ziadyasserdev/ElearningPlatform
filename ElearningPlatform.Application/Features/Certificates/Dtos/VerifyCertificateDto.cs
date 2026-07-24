using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Dtos
{
    public class VerifyCertificateDto
    {
        public bool IsValid { get; set; }

        public bool IsRevoked { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public string CourseTitle { get; set; } = string.Empty;

        public string InstructorName { get; set; } = string.Empty;

        public string CertificateNumber { get; set; } = string.Empty;

        public DateTime IssuedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public string? RevokedReason { get; set; }
    }
}
