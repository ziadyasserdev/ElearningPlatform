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

namespace ElearningPlatform.Application.Features.Questions.Queries.GetQuestionById
{
    public class GetQuestionByIdQueryHandler
    : IRequestHandler<GetQuestionByIdQuery,
        Result<QuestionDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetQuestionByIdQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<QuestionDetailsDto>> Handle(
            GetQuestionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var question = await unitOfWork.Questions.Query()
                .Where(x =>
                    x.Id == request.Id &&
                    x.Exam.Course.Instructor.UserId == userId)
                .Select(x => new QuestionDetailsDto
                {
                    Id = x.Id,
                    QuestionText = x.QuestionText,
                    QuestionType = x.QuestionType.ToString(),
                    Score = x.Score,
                    OrderIndex = x.OrderIndex
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (question is null)
            {
                return Result<QuestionDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Question not found");
            }

            return Result<QuestionDetailsDto>.Success(question);
        }
    }
}
