using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Commands.FinishExam
{
    public class FinishExamCommand : IRequest<Result<string>>
    {
        public int AttemptId { get; set; }
    }
}
