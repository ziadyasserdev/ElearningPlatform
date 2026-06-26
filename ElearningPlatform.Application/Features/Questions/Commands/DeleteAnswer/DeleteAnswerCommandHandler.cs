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

namespace ElearningPlatform.Application.Features.Questions.Commands.DeleteAnswer
{
    public class DeleteAnswerCommandHandler
    : IRequestHandler<DeleteAnswerCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteAnswerCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            DeleteAnswerCommand request,
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

            unitOfWork.Answers.Delete(answer);

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                "Answer deleted successfully");
        }
    }
}
