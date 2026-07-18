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

namespace ElearningPlatform.Application.Features.Assignments.Queries.GetAssignmentById
{
    public class GetAssignmentByIdQueryHandler
      : IRequestHandler<GetAssignmentByIdQuery, Result<AssignmentDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAssignmentByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<AssignmentDetailsDto>> Handle(
            GetAssignmentByIdQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<AssignmentDetailsDto>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");


            var userId = currentUserService.UserId;
            var assignment = await unitOfWork.Assignments
                .Query()
                .Where(a => a.Id == request.Id && !a.IsDeleted 
                && a.Course.Instructor.UserId == userId)
                .Select(a => new AssignmentDetailsDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    MaxScore = a.MaxScore,
                    OpenAt = a.OpenAt,
                    DueDate = a.DueDate,
                    AllowLateSubmission = a.AllowLateSubmission,
                    IsPublished = a.IsPublished,
                    CourseTitle = a.Course.Title,
                    SubmissionCount = a.Submissions.Count
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (assignment is null)
            {
                return Result<AssignmentDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            return Result<AssignmentDetailsDto>.Success(assignment);
        }
    }
}
