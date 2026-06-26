using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Commands.DeleteSection
{
    public class DeleteSectionCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }
        public DeleteSectionCommand(int id)
        {
            Id = id;
        }
    }
}
