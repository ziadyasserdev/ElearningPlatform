using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.UpdateVideoComment
{
    public class UpdateVideoCommentCommand : IRequest<Result<string>>
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
