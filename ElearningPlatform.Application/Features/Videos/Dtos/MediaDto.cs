using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Dtos
{
    public class MediaDto
    {
        public string OriginalName { get; set; }
        public string FakeName { get; set; }
        public string ContentType { get; set; }
        public string Url { get; set; }
        public long Size { get; set; }
    }
}
