using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Commands.SubmitAnswer
{
    public class SubmitAnswerCommand : IRequest<Result<string>>
    {
        public int AttemptId { get; set; }

        public int QuestionId { get; set; }

        public int? SelectedAnswerId { get; set; }

        public string? TextAnswer { get; set; }
    }
}
