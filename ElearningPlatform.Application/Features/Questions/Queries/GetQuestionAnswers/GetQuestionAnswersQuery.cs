using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Queries.GetQuestionAnswers
{
    public class GetQuestionAnswersQuery
    : IRequest<Result<List<AnswerDto>>>
    {
        public int QuestionId { get; set; }
    }
    public class AnswerDto
    {
        public int Id { get; set; }

        public string AnswerText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        public int OrderIndex { get; set; }
    }
}
