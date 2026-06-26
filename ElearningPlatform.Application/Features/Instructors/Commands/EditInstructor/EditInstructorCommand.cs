using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.EditInstructor
{
    public class EditInstructorCommand:IRequest<Result<string>>
    {
        public string? Bio { get; set; }
        public string? Specialization { get; set; }
        public int? ExperienceYears { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}
