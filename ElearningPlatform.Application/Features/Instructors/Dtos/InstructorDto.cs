using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Dtos
{
    public class InstructorDto
    {
       public int Id { get; set; }
        public string FullName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Bio { get; set; }
        public string Specialization { get; set; }
        public int ExperienceYears { get; set; }
        public decimal Rating { get; set; }
    }
}
