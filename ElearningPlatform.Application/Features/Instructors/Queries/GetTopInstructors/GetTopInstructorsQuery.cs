using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Instructors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetTopInstructors
{
    public class GetTopInstructorsQuery:IRequest<Result<List<InstructorDto>>>
    {
        public int Count { get; set; } = 5;
        public GetTopInstructorsQuery(int c)
        {
            Count = c;
        }
    }
}
