using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Enrollments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetCourseProgress
{
    public class GetCourseProgressQueryHandler : IRequestHandler<GetCourseProgressQuery, Result<CourseProgressDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetCourseProgressQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<CourseProgressDto>> Handle(GetCourseProgressQuery request, CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<CourseProgressDto>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var userId = currentUserService.UserId;
            var enrollment = await unitOfWork.Enrollments.Query()
               .FirstOrDefaultAsync(
                   e => e.CourseId == request.CourseId &&
                        e.StudentId == userId,
                   cancellationToken);

            if (enrollment is null)
            {
                return Result<CourseProgressDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not enrolled in this course");
            }

            var totalVideos = await unitOfWork.Videos.Query()
                .CountAsync(
                    v => !v.IsDeleted &&
                         v.Lesson.Section.CourseId == request.CourseId,
                    cancellationToken);

            var completedVideos = await unitOfWork.VideoProgresses.Query()
                .CountAsync(
                    p => p.UserId == userId &&
                         p.IsCompleted &&
                         p.Video.Lesson.Section.CourseId == request.CourseId,
                    cancellationToken);

            var dto = new CourseProgressDto
            {
                CourseId = request.CourseId,
                ProgressPercentage = enrollment.ProgressPercentage,
                TotalVideos = totalVideos,
                CompletedVideos = completedVideos,
                RemainingVideos = totalVideos - completedVideos,
                IsCompleted = enrollment.Status ==
                              Domain.Enums.EnrollmentStatus.Completed
            };

            return Result<CourseProgressDto>.Success(dto);
        }
    }
}
