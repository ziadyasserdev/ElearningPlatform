using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Queries.GetQuestionAnswers
{
    public class GetQuestionAnswersQueryHandler
     : IRequestHandler<GetQuestionAnswersQuery,
         Result<List<AnswerDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetQuestionAnswersQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<AnswerDto>>> Handle(
            GetQuestionAnswersQuery request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var answers = await unitOfWork.Answers.Query()
                .Where(x =>
                    x.QuestionId == request.QuestionId &&
                    x.Question!.Exam.Course.Instructor.UserId == userId)
                .OrderBy(x => x.OrderIndex)
                .Select(x => new AnswerDto
                {
                    Id = x.Id,
                    AnswerText = x.AnswerText,
                    IsCorrect = x.IsCorrect,
                    OrderIndex = x.OrderIndex
                })
                .ToListAsync(cancellationToken);

            return Result<List<AnswerDto>>
                .Success(answers);
        }
    }
}
