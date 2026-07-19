using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Assignments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Queries.GetAssignmentStatistics
{
    public record GetAssignmentStatisticsQuery(int Id)
       : IRequest<Result<AssignmentStatisticsDto>>;
}
