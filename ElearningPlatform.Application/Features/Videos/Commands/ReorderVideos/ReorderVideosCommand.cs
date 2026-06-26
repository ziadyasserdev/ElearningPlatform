using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.ReorderVideos
{
    public class ReorderVideosCommand : IRequest<Result<string>>
    {
        public int SectionId { get; set; }

        public List<VideoOrderDto> Videos { get; set; }
    }

    public class VideoOrderDto
    {
        public int VideoId { get; set; }
        public int NewOrder { get; set; }
    }
}
