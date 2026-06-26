using ElearningPlatform.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ElearningPlatform.Application.Features.Videos.Commands.UpdateThumbnail
{
    public class UpdateThumbnailCommand : IRequest<Result<int>>
    {
        public int VideoId { get; set; }
        public IFormFile Thumbnail { get; set; }
    }
}
