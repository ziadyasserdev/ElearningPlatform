using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.BulkDeleteVideos
{
    public class BulkDeleteVideosCommand : IRequest<Result<string>>
    {
        public List<int> VideoIds { get; set; }
    }
}
