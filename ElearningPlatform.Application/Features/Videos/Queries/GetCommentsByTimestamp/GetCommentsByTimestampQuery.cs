using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Videos.Queries.GetVideoComments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.GetCommentsByTimestamp
{
    public class GetCommentsByTimestampQuery : IRequest<Result<List<VideoCommentDto>>>
    {
        public int VideoId { get; set; }
        public int? Timestamp { get; set; }
    }
}
