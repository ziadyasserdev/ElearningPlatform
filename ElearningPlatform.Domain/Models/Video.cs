using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Video : BaseEntity
    {
        public string Title { get; set; }
        public string FileUrl { get; set; }
        public int LessonId { get; set; }
        public string ?ThumbnailUrl { get; set; }
        public int Duration { get; set; }
        public long FileSize { get; set; }
        public string Format { get; set; }
        public int Order { get; set; }
        public VideoProcessingStatus ProcessingStatus { get; set; }
        public string UploadedBy { get; set; }

        public ApplicationUser Uploader { get; set; }
        public Lesson Lesson { get; set; }
        public ICollection<VideoProgress> VideoProgresses { get; set; }

    }
}
