using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Commands.UpdateGrade
{
    public record UpdateGradeCommand(
     int SubmissionId,
     int Score,
     string? Feedback)
     : IRequest<Result<string>>;
}
