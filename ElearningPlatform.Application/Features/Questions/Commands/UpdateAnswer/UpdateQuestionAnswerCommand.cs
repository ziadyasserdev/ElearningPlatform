using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Commands.UpdateAnswer
{
    public class UpdateQuestionAnswerCommand : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public string AnswerText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}
