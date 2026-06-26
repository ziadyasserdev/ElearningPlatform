using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.GetVideoComments
{
    public class GetVideoCommentsQuery : IRequest<Result<List<VideoCommentDto>>>
    {
        public int VideoId { get; set; }
    }
    public class VideoCommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public int? TimestampSeconds { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<VideoCommentDto> Replies { get; set; }
    }
}
