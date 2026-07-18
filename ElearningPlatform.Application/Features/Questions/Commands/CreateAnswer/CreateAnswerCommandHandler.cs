using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Commands.CreateAnswer
{
    public class CreateAnswerCommandHandler
     : IRequestHandler<CreateAnswerCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateAnswerCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            CreateAnswerCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");

            var userId = currentUserService.UserId;

            var question = await unitOfWork.Questions.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.QuestionId &&
                    x.Exam.Course.Instructor.UserId == userId,
                    cancellationToken);

            if (question is null)
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Question not found");

           
            if (request.IsCorrect)
            {
                var hasCorrectAnswer = await unitOfWork.Answers.Query()
                    .AnyAsync(x =>
                        x.QuestionId == request.QuestionId &&
                        x.IsCorrect,
                        cancellationToken);

                if (hasCorrectAnswer)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        "Question already has a correct answer");
                }
            }

            var orderIndex = await unitOfWork.Answers.Query()
                .CountAsync(x =>
                    x.QuestionId == request.QuestionId,
                    cancellationToken);

            var answer = new Answer
            {
                QuestionId = request.QuestionId,
                AnswerText = request.AnswerText,
                IsCorrect = request.IsCorrect,
                OrderIndex = orderIndex + 1,

                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName
            };

            await unitOfWork.Answers.AddAsync(answer);

            await unitOfWork.SaveAsync();

            return Result<int>.Success(answer.Id);
        }
    }
}
