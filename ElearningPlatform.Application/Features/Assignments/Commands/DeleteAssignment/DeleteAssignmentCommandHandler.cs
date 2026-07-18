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

namespace ElearningPlatform.Application.Features.Assignments.Commands.DeleteAssignment
{

    public class DeleteAssignmentCommandHandler
    : IRequestHandler<DeleteAssignmentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteAssignmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            DeleteAssignmentCommand request,
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
                    "You are not authorized to delete this assignment.");
            }

          
            var hasSubmissions = await unitOfWork.Submissions
                .Query()
                .AnyAsync(s =>
                    s.AssignmentId == assignment.Id &&
                    !s.IsDeleted,
                    cancellationToken);

            if (hasSubmissions)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Assignment cannot be deleted because students have already submitted.");
            }

            
            assignment.IsDeleted = true;
            assignment.DeletedAt = DateTime.Now;
            assignment.DeletedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Assignment deleted successfully.");
        }
    }
}
