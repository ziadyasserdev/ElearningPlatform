using ElearningPlatform.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.UpdateVideo
{
    public class UpdateVideoCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public int Duration { get; set; }

        public IFormFile? File { get; set; }
    }
}
