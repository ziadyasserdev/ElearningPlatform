using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Dtos
{
    public class CertificateAnalyticsDto
    {
        public string Month { get; set; } = string.Empty;

        public int GeneratedCertificates { get; set; }

        public int Downloads { get; set; }
    }
}
