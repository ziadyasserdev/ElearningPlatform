using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Dtos
{
    public class CertificateStatisticsDto
    {
        public int TotalCertificates { get; set; }

        public int ActiveCertificates { get; set; }

        public int RevokedCertificates { get; set; }

        public int TotalDownloads { get; set; }

        public int GeneratedThisMonth { get; set; }
    }
}
