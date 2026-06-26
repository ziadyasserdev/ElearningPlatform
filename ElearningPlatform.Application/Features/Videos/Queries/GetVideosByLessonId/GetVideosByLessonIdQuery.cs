using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Videos.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.GetVideosByLessonId
{
    public class GetVideosByLessonIdQuery : IRequest<Result<List<VideoListDto>>>
    {
        public int LessonId { get; set; }
    }
}
