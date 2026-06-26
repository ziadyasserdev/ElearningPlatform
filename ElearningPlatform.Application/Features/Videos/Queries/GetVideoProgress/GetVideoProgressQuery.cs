using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.GetVideoProgress
{
    public class GetVideoProgressQuery : IRequest<Result<VideoProgressDto>>
    {
        public int VideoId { get; set; }
    }
    public class VideoProgressDto
    {
        public int WatchedSeconds { get; set; }
        public double ProgressPercentage { get; set; }
        public bool IsCompleted { get; set; }
        public int LastWatchedSecond { get; set; }
    }
}
