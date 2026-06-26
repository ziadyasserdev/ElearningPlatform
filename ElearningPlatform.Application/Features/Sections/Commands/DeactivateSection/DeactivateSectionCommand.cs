using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Commands.DeactivateSection
{
    public class DeactivateSectionCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public DeactivateSectionCommand(int id)
        {
            Id = id;
        }
    }
}
