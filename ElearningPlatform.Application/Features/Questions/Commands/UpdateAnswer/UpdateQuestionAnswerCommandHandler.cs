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

namespace ElearningPlatform.Application.Features.Questions.Commands.UpdateAnswer
{
    public class UpdateQuestionAnswerCommandHandler
    : IRequestHandler<UpdateQuestionAnswerCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateQuestionAnswerCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            UpdateQuestionAnswerCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var answer = await unitOfWork.Answers.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.Id &&
                    x.Question!.Exam.Course.Instructor.UserId == userId,
                    cancellationToken);

            if (answer is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Answer not found");
            }

            if (request.IsCorrect)
            {
                var anotherCorrect = await unitOfWork.Answers.Query()
                    .AnyAsync(x =>
                        x.QuestionId == answer.QuestionId &&
                        x.Id != answer.Id &&
                        x.IsCorrect,
                        cancellationToken);

                if (anotherCorrect)
                {
                    return Result<string>.Failure(
                        ResultStatus.Conflict,
                        "Question already has a correct answer");
                }
            }

            answer.AnswerText = request.AnswerText;
            answer.IsCorrect = request.IsCorrect;

            answer.UpdatedAt = DateTime.UtcNow;
            answer.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Answer updated successfully");
        }
    }
}
