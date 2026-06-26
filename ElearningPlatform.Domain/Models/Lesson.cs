using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Lesson : BaseEntity
    {
        public int SectionId { get; set; }
     
     
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Duration { get; set; }
        public int OrderIndex { get; set; }
        public bool IsPreview { get; set; }
        public bool IsPublished { get; set; }

        public Section Section { get; set; }
        public ICollection<Video> Videos { get; set; }
        //public ICollection<LessonProgress> LessonProgresses { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
    }
}
