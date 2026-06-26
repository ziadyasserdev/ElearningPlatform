using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.RestoreVideo
{
    public class RestoreVideoCommand : IRequest<Result<int>>
    {
        public int VideoId { get; set; }
    }
}
