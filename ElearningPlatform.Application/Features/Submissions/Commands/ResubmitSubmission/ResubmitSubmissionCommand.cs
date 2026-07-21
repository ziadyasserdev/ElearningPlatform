using ElearningPlatform.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Commands.ResubmitSubmission
{
    public record ResubmitSubmissionCommand(
      int SubmissionId,
      IFormFile File)
      : IRequest<Result<string>>;
}
