using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Commands.UpdateQuestion
{
    public class UpdateQuestionCommand : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public QuestionType QuestionType { get; set; }

        public decimal Score { get; set; }
    }
}
