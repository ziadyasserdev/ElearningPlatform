using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Videos.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.GetVideoById
{
    public class GetVideoByIdQuery : IRequest<Result<VideoDetailsDto>>
    {
        public int Id { get; set; }
    }
}
