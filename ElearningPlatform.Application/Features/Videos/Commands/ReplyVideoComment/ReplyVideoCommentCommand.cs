using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.ReplyVideoComment
{
    public class ReplyVideoCommentCommand : IRequest<Result<string>>
    {
        public int VideoId { get; set; }
        public int CommentId { get; set; }
        public string Content { get; set; }
    }
}
