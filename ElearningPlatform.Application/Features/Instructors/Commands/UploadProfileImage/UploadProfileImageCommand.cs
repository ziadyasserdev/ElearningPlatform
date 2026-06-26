using ElearningPlatform.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.UploadProfileImage
{
    public class UploadProfileImageCommand:IRequest<Result<string>>
    {
   
        public IFormFile Image { get; set; }
    }
}
