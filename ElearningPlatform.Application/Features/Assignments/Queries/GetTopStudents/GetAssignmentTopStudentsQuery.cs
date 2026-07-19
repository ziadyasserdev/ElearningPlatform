using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Assignments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Queries.GetTopStudents
{
    public record GetAssignmentTopStudentsQuery(
     int AssignmentId,
     int Count = 10)
     : IRequest<Result<List<TopStudentDto>>>;
}
