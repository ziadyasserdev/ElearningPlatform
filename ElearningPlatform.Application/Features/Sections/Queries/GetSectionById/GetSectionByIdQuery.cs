using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Sections.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Queries.GetSectionById
{
    public class GetSectionByIdQuery:IRequest<Result<SectionDetailsDto>>
    {
         public int Id { get; set; }
        public GetSectionByIdQuery(int id)
        {
            Id = id;
        }
    }
}
