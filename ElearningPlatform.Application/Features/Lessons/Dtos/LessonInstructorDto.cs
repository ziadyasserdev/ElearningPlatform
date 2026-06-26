using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Dtos
{
    public class LessonInstructorDto:LessonForSectionDto
    {
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
    }
}
