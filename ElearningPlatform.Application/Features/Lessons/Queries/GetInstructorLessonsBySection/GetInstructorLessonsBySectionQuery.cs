using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Lessons.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Queries.GetInstructorLessonsBySection
{
    public class GetInstructorLessonsBySectionQuery : IRequest<Result<List<LessonInstructorDto>>>
    {
        public int SectionId { get; set; }
    }
}
