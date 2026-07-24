using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Dtos
{
    public class DownloadCertificateDto
    {
        public string CertificateUrl { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;
    }
}
