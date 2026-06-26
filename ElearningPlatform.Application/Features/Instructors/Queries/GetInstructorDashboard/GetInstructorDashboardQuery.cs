using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Instructors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorDashboard
{
    public class GetInstructorDashboardQuery:IRequest<Result<InstructorDashboardDto>>
    {
        public int InstructorId { get; set; }

        public GetInstructorDashboardQuery(int instructorId)
        {
            InstructorId = instructorId;
        }
    }
}
