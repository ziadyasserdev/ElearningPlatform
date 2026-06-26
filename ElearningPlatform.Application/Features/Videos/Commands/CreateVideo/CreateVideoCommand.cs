using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Videos.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.CreateVideo
{
    public class CreateVideoCommand:IRequest<Result<int>>
    {
        public string Title { get; set; }
        public IFormFile File { get; set; }
        public int LessonId { get; set; } 
        public int Duration { get; set; }
    }
}
