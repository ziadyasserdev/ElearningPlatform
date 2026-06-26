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
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxScore { get; set; }
        public DateTime DueDate { get; set; }

        public Course Course { get; set; }
        public ICollection<Submission> Submissions { get; set; }
    }
}
