using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Commands.ReturnSubmission
{
    public record ReturnSubmissionCommand(
       int SubmissionId,
       string Reason
   ) : IRequest<Result<string>>;
}
