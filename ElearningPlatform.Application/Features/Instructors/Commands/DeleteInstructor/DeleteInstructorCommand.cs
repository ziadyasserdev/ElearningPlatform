using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.DeleteInstructor
{
    public class DeleteInstructorCommand:IRequest<Result<string>>
    {
        public string UserId { get; set; }
    }
}
