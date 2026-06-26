using ElearningPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class VideoProgress
    {
        public int Id { get; set; } 

        public string UserId { get; set; }
        public int VideoId { get; set; }

      
        public int WatchedSeconds { get; set; }
        public double ProgressPercentage { get; set; }

      
        public bool IsCompleted { get; set; }

     
        public int LastWatchedSecond { get; set; }

      
        public DateTime LastWatchedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

      
        public int VideoDuration { get; set; }

     
        public ApplicationUser User { get; set; }
        public Video Video { get; set; }
    }
}
