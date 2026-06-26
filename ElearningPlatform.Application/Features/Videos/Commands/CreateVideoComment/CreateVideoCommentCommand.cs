using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.CreateVideoComment
{
    public class CreateVideoCommentCommand : IRequest<Result<string>>
    {
        public int VideoId { get; set; }
        public string Content { get; set; }
        public int? TimestampSeconds { get; set; }
    }
}
