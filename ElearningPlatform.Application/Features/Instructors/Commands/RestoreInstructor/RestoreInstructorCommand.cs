using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.RestoreInstructor
{
    public class RestoreInstructorCommand:IRequest<Result<string>>
    {
        public string UserId { get; set; }
        public RestoreInstructorCommand(string id)
        {
            UserId = id;
        }
    }
}
