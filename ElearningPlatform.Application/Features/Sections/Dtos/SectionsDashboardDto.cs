using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Dtos
{
    public class SectionsDashboardDto
    {
        public int TotalSections { get; set; }
        public int ActiveSections { get; set; }
        public int InactiveSections { get; set; }
        public int DeletedSections { get; set; }
    }
}
