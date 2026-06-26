using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorStudentsCount
{
    public class GetInstructorStudentsCountQuery:IRequest<Result<int>>
    {
        public int InstructorId { get; set; }

        public GetInstructorStudentsCountQuery(int instructorId)
        {
            InstructorId = instructorId;
        }
    }
}
