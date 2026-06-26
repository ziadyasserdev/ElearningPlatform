using ElearningPlatform.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.EditCategoryIcon
{
    public class EditCategoryIconCommand:IRequest<Result<string>>
    {
        public int CategoryId { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
