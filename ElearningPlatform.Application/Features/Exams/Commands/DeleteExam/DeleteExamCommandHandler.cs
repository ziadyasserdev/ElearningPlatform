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

namespace ElearningPlatform.Application.Features.Exams.Commands.DeleteExam
{
    public class DeleteExamCommandHandler
      : IRequestHandler<DeleteExamCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteExamCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            DeleteExamCommand request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var exam = await unitOfWork.Exams.Query()
                .Include(x => x.Course)
                .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(x => x.Id == request.Id,
                    cancellationToken);

            if (exam is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Exam not found");

            if (exam.Course.Instructor.UserId != currentUserService.UserId)
                return Result<string>.Failure(ResultStatus.Forbidden, "Not allowed");

            var hasAttempts = await unitOfWork.ExamAttempts.Query()
                .AnyAsync(x => x.ExamId == request.Id, cancellationToken);

            if (hasAttempts)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Cannot delete exam with attempts");

            unitOfWork.Exams.Delete(exam);

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Exam deleted successfully");
        }
    }
}
