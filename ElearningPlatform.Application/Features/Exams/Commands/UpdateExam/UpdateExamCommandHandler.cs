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

namespace ElearningPlatform.Application.Features.Exams.Commands.UpdateExam
{
    public class UpdateExamCommandHandler
       : IRequestHandler<UpdateExamCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateExamCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            UpdateExamCommand request,
            CancellationToken cancellationToken)
        {if(!currentUserService.IsAuthenticated)
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
            if (request.StartAt.HasValue && request.EndAt.HasValue && request.EndAt <= request.StartAt) 
            { return Result<string>
                    .Failure(ResultStatus.Failure
                    , "End date must be greater than start date"); }
            exam.Title = request.Title ?? exam.Title;
            exam.Description = request.Description ?? exam.Description;
            exam.DurationMinutes = request.DurationMinutes;
            exam.TotalScore = request.TotalScore;
            exam.PassingScore = request.PassingScore;

            exam.StartAt = request.StartAt;
                 exam.EndAt = request.EndAt;
            exam.UpdatedAt = DateTime.Now;
            exam.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success( "Exam updated successfully");
        }
    }
}
