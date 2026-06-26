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

namespace ElearningPlatform.Application.Features.Questions.Commands.DeleteQuestion
{
    public class DeleteQuestionCommandHandler
    : IRequestHandler<DeleteQuestionCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteQuestionCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            DeleteQuestionCommand request,
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

            unitOfWork.Questions.Delete(question);

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                "Question deleted successfully");
        }
    }
}
