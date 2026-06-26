using ElearningPlatform.Application.Features.Videos.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Dtos
{
    public class LessonForSectionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Duration { get; set; }
        public int OrderIndex { get; set; }
        public List<VideoDto> Videos { get; set; }
    }
}
