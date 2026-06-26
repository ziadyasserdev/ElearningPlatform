using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Sections.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Queries.GetSectionsByCourse
{
    public class GetSectionsByCourseQuery:IRequest<Result<List<SectionDto>>>
    {
        public int CourseId { get; set; }
        public GetSectionsByCourseQuery(int id)
        {
            CourseId = id;  
        }
    }
}
