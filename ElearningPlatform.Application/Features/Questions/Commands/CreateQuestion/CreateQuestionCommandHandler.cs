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

namespace ElearningPlatform.Application.Features.Questions.Commands.CreateQuestion
{
    public class CreateQuestionCommandHandler
        : IRequestHandler<CreateQuestionCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateQuestionCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            CreateQuestionCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");
            }

            var userId = currentUserService.UserId;

            var exam = await unitOfWork.Exams.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.ExamId &&
                    x.Course.Instructor.UserId == userId,
                    cancellationToken);

            if (exam is null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Exam not found");
            }

            var orderIndex = await unitOfWork.Questions.Query()
                .CountAsync(x =>
                    x.ExamId == request.ExamId,
                    cancellationToken);

            var question = new Question
            {
                ExamId = request.ExamId,
                QuestionText = request.QuestionText,
                QuestionType = request.QuestionType,
                Score = request.Score,

                OrderIndex = orderIndex + 1,
               
                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName
            };

            await unitOfWork.Questions.AddAsync(question);

            await unitOfWork.SaveAsync();

            return Result<int>.Success(question.Id);
        }
    }
}
