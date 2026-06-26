using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.AddInstructor
{
    public class AddInstructorCommand:IRequest<Result<string>>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public string LinkedInUrl { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
    }
}
