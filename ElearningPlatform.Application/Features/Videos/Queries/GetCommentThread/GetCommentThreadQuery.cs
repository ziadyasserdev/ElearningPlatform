using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.GetCommentThread
{
    public class GetCommentThreadQuery : IRequest<Result<VideoCommentThreadDto>>
    {
        public int Id { get; set; }
    }
    public class VideoCommentThreadDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<VideoCommentThreadDto> Replies { get; set; }
    }
}
