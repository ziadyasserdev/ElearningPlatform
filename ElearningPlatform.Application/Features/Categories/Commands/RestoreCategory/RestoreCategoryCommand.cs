using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.RestoreCategory
{
    public class RestoreCategoryCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }

        public RestoreCategoryCommand(int id)
        {
            Id = id;
        }
    }
}
