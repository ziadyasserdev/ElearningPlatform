using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Dtos
{
    public class AssignmentListDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int MaxScore { get; set; }

        public DateTime OpenAt { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsPublished { get; set; }

        public int SubmissionCount { get; set; }
    }
}
