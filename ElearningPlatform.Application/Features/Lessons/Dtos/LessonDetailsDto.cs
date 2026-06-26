using ElearningPlatform.Application.Features.Videos.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Dtos
{
    public class LessonDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Duration { get; set; }
        public bool IsPreview { get; set; }

        public List<VideoDto> Videos { get; set; }
    }
}
