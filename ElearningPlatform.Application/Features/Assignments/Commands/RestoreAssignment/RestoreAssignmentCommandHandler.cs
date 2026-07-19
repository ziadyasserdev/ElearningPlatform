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

namespace ElearningPlatform.Application.Features.Assignments.Commands.RestoreAssignment
{
    public class RestoreAssignmentCommandHandler
     : IRequestHandler<RestoreAssignmentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RestoreAssignmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            RestoreAssignmentCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId;

            var assignment = await unitOfWork.Assignments
                .Query()
                .Include(a => a.Course)
                .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(a =>
                    a.Id == request.Id,
                    cancellationToken);

            if (assignment is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            if (assignment.Course.Instructor.UserId != userId)
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to restore this assignment.");
            }
            var titleExists = await unitOfWork.Assignments
                .Query()
                .AnyAsync(a =>
                    a.Id != assignment.Id &&
                    a.CourseId == assignment.CourseId &&
                    a.Title == assignment.Title &&
                    !a.IsDeleted,
                    cancellationToken);

            if (titleExists)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "An assignment with the same title already exists in this course.");
            }

            if (!assignment.IsDeleted)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Assignment is not deleted.");
            }

            assignment.IsDeleted = false;
            assignment.DeletedAt = null;
            assignment.DeletedBy = null;

            assignment.UpdatedAt = DateTime.Now;
            assignment.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Assignment restored successfully.");
        }
    }
}
