using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Dtos
{
    public class GetVideoProgressDto
    {
        public int VideoId { get; set; }
        public int LastWatchedSecond { get; set; }
        public double ProgressPercentage { get; set; }
        public bool IsCompleted { get; set; }
        public int VideoDuration { get; set; }
    }
}
