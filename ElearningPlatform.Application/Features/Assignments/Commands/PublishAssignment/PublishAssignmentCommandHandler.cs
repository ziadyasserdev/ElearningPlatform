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

namespace ElearningPlatform.Application.Features.Assignments.Commands.PublishAssignment
{
    public class PublishAssignmentCommandHandler
      : IRequestHandler<PublishAssignmentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public PublishAssignmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            PublishAssignmentCommand request,
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
                    a.Id == request.Id &&
                    !a.IsDeleted,
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
                    "You are not authorized to publish this assignment.");
            }

            if (assignment.IsPublished)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Assignment is already published.");
            }

            if (assignment.OpenAt >= assignment.DueDate)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Open date must be earlier than due date.");
            }

            if (assignment.DueDate <= DateTime.Now)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Assignment due date has already passed.");
            }

            if (assignment.MaxScore <= 0)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Assignment max score must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(assignment.Title))
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Assignment title is required.");
            }

            if (string.IsNullOrWhiteSpace(assignment.Description))
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Assignment description is required.");
            }

            assignment.IsPublished = true;
         assignment.PublishedAt = DateTime.Now;
            assignment.UpdatedAt = DateTime.Now;
            assignment.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Assignment published successfully.");
        }
    }
}
