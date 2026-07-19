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

namespace ElearningPlatform.Application.Features.Assignments.Commands.ReopenAssignment
{
    public class ReopenAssignmentCommandHandler
     : IRequestHandler<ReopenAssignmentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ReopenAssignmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            ReopenAssignmentCommand request,
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
                    "You are not authorized to reopen this assignment.");
            }

            if (!assignment.IsPublished)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Only published assignments can be reopened.");
            }

            if (!assignment.IsClosed)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Assignment is already open.");
            }

          
            if (assignment.DueDate <= DateTime.UtcNow)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "The assignment deadline has passed. Extend the deadline before reopening.");
            }

            assignment.IsClosed = false;
            assignment.UpdatedAt = DateTime.UtcNow;
            assignment.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Assignment reopened successfully.");
        }
    }
}
