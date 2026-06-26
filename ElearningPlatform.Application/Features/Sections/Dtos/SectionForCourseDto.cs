using ElearningPlatform.Application.Features.Lessons.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Dtos
{
    public class SectionForCourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int OrderIndex { get; set; }

        public List<LessonDetailsDto> Lessons { get; set; }
    }
}
