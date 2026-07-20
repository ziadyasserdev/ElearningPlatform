using ElearningPlatform.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Commands.CreateSubmission
{
    public record CreateSubmissionCommand(
       int AssignmentId,
       IFormFile File,
       string? Comment)
       : IRequest<Result<int>>;
}