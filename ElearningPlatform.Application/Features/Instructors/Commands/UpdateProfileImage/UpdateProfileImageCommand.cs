using ElearningPlatform.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.UpdateProfileImage
{
    public class UpdateProfileImageCommand:IRequest<Result<string>>
    {
        public IFormFile formFile { get; set; }
         
    }
}
