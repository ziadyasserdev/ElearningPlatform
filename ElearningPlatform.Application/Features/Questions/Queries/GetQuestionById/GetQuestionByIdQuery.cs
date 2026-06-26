using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Questions.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Queries.GetQuestionById
{
    public class GetQuestionByIdQuery
     : IRequest<Result<QuestionDetailsDto>>
    {
        public int Id { get; set; }
    }
}
