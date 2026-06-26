using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Questions.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Queries.GetExamQuestions
{
    public class GetExamQuestionsQuery
     : IRequest<Result<List<QuestionDetailsDto>>>
    {
        public int ExamId { get; set; }
    }
}
