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

namespace ElearningPlatform.Application.Features.Questions.Commands.UpdateQuestion
{
    public class UpdateQuestionCommandHandler
     : IRequestHandler<UpdateQuestionCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateQuestionCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            UpdateQuestionCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");
            }

            var userId = currentUserService.UserId;

            var question = await unitOfWork.Questions.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.Id &&
                    x.Exam.Course.Instructor.UserId == userId,
                    cancellationToken);

            if (question is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Question not found");
            }

            question.QuestionText = request.QuestionText;
            question.QuestionType = request.QuestionType;
            question.Score = request.Score;

            question.UpdatedAt = DateTime.UtcNow;
            question.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                "Question updated successfully");
        }
    }
}
