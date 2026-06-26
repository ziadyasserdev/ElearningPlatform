using ElearningPlatform.Application.Features.Lessons.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Dtos
{
    public class SectionDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int OrderIndex { get; set; }
        public int CourseId { get; set; }
        public List<LessonDto> Lessons { get; set; }
    }
}
