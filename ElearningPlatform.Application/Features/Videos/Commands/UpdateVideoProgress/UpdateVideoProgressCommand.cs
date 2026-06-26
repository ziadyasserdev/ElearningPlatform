using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.UpdateVideoProgress
{
    public class UpdateVideoProgressCommand : IRequest<Result<string>>
    {
        public int VideoId { get; set; }
        public int CurrentTimeInSeconds { get; set; }
    }
}
