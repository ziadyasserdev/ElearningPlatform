using ElearningPlatform.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.AddImageProfile
{
    public class AddImageProfileCommand : IRequest<Result<string>>
    {

        public IFormFile? ProfileImage { get; set; }
    }
}
