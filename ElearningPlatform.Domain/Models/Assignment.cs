using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Assignment : BaseEntity
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

       
        public int MaxScore { get; set; }

      
        public DateTime OpenAt { get; set; }

    
        public DateTime DueDate { get; set; }

       
        public bool IsPublished { get; set; }

       
        public bool AllowLateSubmission { get; set; }

       
        public string? AttachmentUrl { get; set; }

        public Course Course { get; set; } = null!;

        public ICollection<Submission> Submissions { get; set; }
            = new List<Submission>();
    }
}
