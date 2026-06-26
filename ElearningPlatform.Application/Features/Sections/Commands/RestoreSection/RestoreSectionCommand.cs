using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Commands.RestoreSection
{
    public class RestoreSectionCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }
        public RestoreSectionCommand(int id)
        {
            Id = id;
        }
    }
}
