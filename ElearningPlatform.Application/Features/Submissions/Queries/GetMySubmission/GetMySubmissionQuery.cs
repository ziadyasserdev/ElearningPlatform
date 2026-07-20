using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Submissions.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.GetMySubmission
{
    public record GetMySubmissionQuery(
    int AssignmentId)
    : IRequest<Result<MySubmissionDto>>;

}
