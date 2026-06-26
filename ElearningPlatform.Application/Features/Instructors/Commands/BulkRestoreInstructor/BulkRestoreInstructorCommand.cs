using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.BulkRestoreInstructor
{
    public class BulkRestoreInstructorCommand:IRequest<Result<string>>
    {
        public List<Guid> InstructorIds { get; set; } = new();
    }
}
