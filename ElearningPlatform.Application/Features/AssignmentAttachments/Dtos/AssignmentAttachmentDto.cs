using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Dtos
{

    public class AssignmentAttachmentDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long FileSize { get; set; }
        public int OrderIndex { get; set; } 
     
        }
}
