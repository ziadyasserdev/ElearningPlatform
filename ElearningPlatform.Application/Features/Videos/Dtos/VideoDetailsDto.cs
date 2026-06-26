using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Dtos
{
     public class VideoDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Duration { get; set; }
        public bool IsPublished { get; set; }
        public int LessonId { get; set; }
    }
}
