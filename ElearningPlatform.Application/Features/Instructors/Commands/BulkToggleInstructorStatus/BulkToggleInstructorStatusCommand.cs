using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.BulkToggleInstructorStatus
{
    public class BulkToggleInstructorStatusCommand:IRequest<Result<string>>
    {
        public List<string> UserIds { get; set; } = new();
        public bool IsActive { get; set; }
    }
}
