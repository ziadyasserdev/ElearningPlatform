using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Lessons.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Queries.GetLessonsBySection
{
    public class GetLessonsBySectionQuery : IRequest<Result<List<LessonForSectionDto>>>
    {
        public int SectionId { get; set; }
       
    }
}
