using ElearningPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class VideoComment
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public int VideoId { get; set; }

        public string Content { get; set; }

        public int? TimestampSeconds { get; set; }

        public int? ParentCommentId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ApplicationUser User { get; set; }
        public Video Video { get; set; }
        public VideoComment ParentComment { get; set; }
        public ICollection<VideoComment> Replies { get; set; }
    }
}
