using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Videos.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.DownloadVideo
{
    public class DownloadVideoQuery:IRequest<Result<FileDownloadDto>>
    {
        public int LessonId { get; set; }
        public int VideoId { get; set; }
    }
}
