using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Dtos
{
    public class MyCertificateDto
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public string InstructorName { get; set; } = string.Empty;

        public string CertificateNumber { get; set; } = string.Empty;

        public DateTime IssuedAt { get; set; }

        public string CertificateUrl { get; set; } = string.Empty;

        public bool IsRevoked { get; set; }

        public int DownloadCount { get; set; }
    }
}
