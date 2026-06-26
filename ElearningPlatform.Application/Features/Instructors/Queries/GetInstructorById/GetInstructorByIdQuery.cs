using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Instructors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorById
{
    public class GetInstructorByIdQuery:IRequest<Result<InstructorDto>>
    {
        public int Id { get; set; }
        public GetInstructorByIdQuery(int id)
        {
            Id = id;
        }
    }
}
