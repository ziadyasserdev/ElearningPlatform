using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Submissions.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.DownloadAllSubmissions
{
    public record DownloadAllSubmissionsQuery(
        int AssignmentId)
        : IRequest<Result<DownloadAllSubmissionsResult>>;
}
