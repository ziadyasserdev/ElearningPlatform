using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Commands.StartExam
{

    public class StartExamCommand : IRequest<Result<int>>
    {
        public int ExamId { get; set; }
    }
}
