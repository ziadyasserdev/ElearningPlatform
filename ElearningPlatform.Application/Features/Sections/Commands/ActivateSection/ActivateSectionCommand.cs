using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Commands.ActivateSection
{
    public class ActivateSectionCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }
        public ActivateSectionCommand(int id)
        {
            this.Id = id;
            
        }
    }
}
