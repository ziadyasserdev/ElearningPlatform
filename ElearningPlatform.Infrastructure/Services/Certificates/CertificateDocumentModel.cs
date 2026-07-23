using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Infrastructure.Services.Certificates
{
    public class CertificateDocumentModel
    {
        public string StudentName { get; set; } = string.Empty;

        public string CourseTitle { get; set; } = string.Empty;

        public string InstructorName { get; set; } = string.Empty;

        public string CertificateNumber { get; set; } = string.Empty;

        public string VerificationCode { get; set; } = string.Empty;

        public DateTime IssuedAt { get; set; }

        public byte[]? QrCode { get; set; }
    }
}
