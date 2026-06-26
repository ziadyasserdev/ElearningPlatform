using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Instructors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.SearchInstructors
{
    public class SearchInstructorsQuery:IRequest<Result<List<InstructorDto>>>
    {
        public string? Name { get; set; }
        public string? Specialization { get; set; }
        public decimal? MinRating { get; set; }
    }
}
