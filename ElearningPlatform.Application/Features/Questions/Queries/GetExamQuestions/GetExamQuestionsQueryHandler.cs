using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Questions.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Queries.GetExamQuestions
{
    public class GetExamQuestionsQueryHandler
    : IRequestHandler<GetExamQuestionsQuery,
        Result<List<QuestionDetailsDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetExamQuestionsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<QuestionDetailsDto>>> Handle(
            GetExamQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var questions = await unitOfWork.Questions.Query()
                .Where(x =>
                    x.ExamId == request.ExamId &&
                    x.Exam.Course.Instructor.UserId == userId)
                .OrderBy(x => x.OrderIndex)
                .Select(x => new QuestionDetailsDto
                {
                    Id = x.Id,
                    QuestionText = x.QuestionText,
                    QuestionType = x.QuestionType.ToString(),
                    Score = x.Score,
                    OrderIndex = x.OrderIndex
                })
                .ToListAsync(cancellationToken);

            return Result<List<QuestionDetailsDto>>
                .Success(questions);
        }
    }
}
