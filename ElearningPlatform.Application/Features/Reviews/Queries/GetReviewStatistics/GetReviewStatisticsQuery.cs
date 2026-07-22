using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Reviews.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Queries.GetReviewStatistics
{
    public class GetReviewStatisticsQuery
     : IRequest<Result<ReviewStatisticsDto>>
    {
    }
}
