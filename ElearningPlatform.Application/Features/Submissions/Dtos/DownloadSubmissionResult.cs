using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Dtos
{
    public class DownloadSubmissionResult
    {
        public byte[] FileBytes { get; set; } = [];

        public string FileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = "application/octet-stream";
    }
}
