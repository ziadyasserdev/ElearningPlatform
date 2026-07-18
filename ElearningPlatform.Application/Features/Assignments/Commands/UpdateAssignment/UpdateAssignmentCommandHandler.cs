using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Assignments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Commands.UpdateAssignment
{

    public class UpdateAssignmentCommandHandler
        : IRequestHandler<UpdateAssignmentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateAssignmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            UpdateAssignmentCommand request,
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
                    "You are not authorized to update this assignment.");
            }

         
            var titleExists = await unitOfWork.Assignments
                .Query()
                .AnyAsync(a =>
                    a.CourseId == assignment.CourseId &&
                    a.Id != assignment.Id &&
                    a.Title == request.Title &&
                    !a.IsDeleted,
                    cancellationToken);

            if (titleExists)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Assignment title already exists.");
            }

           
            var hasSubmissions = await unitOfWork.Submissions
                .Query()
                .AnyAsync(s =>
                    s.AssignmentId == assignment.Id &&
                    !s.IsDeleted,
                    cancellationToken);

            if (hasSubmissions)
            {
              
                if (assignment.MaxScore != request.MaxScore ||
                    assignment.OpenAt != request.OpenAt ||
                    assignment.DueDate != request.DueDate)
                {
                    return Result<string>.Failure(
                        ResultStatus.Failure,
                        "Max score, open date and due date cannot be modified because students have already submitted.");
                }

            
                assignment.Title = request.Title;
                assignment.Description = request.Description;
                assignment.AllowLateSubmission = request.AllowLateSubmission;
            }
            else
            {
              
                assignment.Title = request.Title;
                assignment.Description = request.Description;
                assignment.MaxScore = request.MaxScore;
                assignment.OpenAt = request.OpenAt;
                assignment.DueDate = request.DueDate;
                assignment.AllowLateSubmission = request.AllowLateSubmission;
            }

            assignment.UpdatedAt = DateTime.UtcNow;
            assignment.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Assignment updated successfully.");
        }
    }
}
