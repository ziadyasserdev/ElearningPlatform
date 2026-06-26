using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Sections.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Queries.GetSectionsCountByCourse
{
    public class GetSectionsCountByCourseQuery:IRequest<Result<SectionsDashboardDto>>
    {
        public int Id { get; set; }
        public GetSectionsCountByCourseQuery(int id)
        {
            this.Id = id;   
        }
    }
}
