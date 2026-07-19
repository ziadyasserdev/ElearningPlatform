using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class AssignmentAttachment : BaseEntity
    {
        public int AssignmentId { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FileUrl { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public int OrderIndex { get; set; } = 1;

        public Assignment Assignment { get; set; } = null!;
    }
}
