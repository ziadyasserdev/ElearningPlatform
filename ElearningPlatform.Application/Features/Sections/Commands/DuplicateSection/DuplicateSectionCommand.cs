using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Commands.DuplicateSection
{
    public class DuplicateSectionCommand:IRequest<Result<int>>
    {
           public int Id { get; set; }
        public DuplicateSectionCommand(int id)
        {
            this.Id = id;
        }
    }
}
