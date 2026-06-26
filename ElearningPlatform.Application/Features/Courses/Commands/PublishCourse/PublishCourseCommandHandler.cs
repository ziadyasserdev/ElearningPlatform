using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.PublishCourse
{
    public class PublishCourseCommandHandler
       : IRequestHandler<PublishCourseCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public PublishCourseCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            PublishCourseCommand request,
            CancellationToken cancellationToken)
        {
            var instructor = await unitOfWork.Instructors.Query()
                .FirstOrDefaultAsync(
                    x => x.UserId == currentUserService.UserId,
                    cancellationToken);

            if (instructor is null)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Instructor not found");
            }

            var course = await unitOfWork.Courses.Query()
                .Include(c => c.Sections)
                    .ThenInclude(s => s.Lessons)
                        .ThenInclude(l => l.Videos)
                .FirstOrDefaultAsync(
                    c => c.Id == request.CourseId
                      && c.InstructorId == instructor.Id
                      && !c.IsDeleted,
                    cancellationToken);

            if (course is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Course not found");
            }

            if (course.Status == CourseStatus.Published)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Course already published");
            }

            if (string.IsNullOrWhiteSpace(course.Description))
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Course description is required");
            }

            if (string.IsNullOrWhiteSpace(course.ShortDescription))
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Course short description is required");
            }

            if (string.IsNullOrWhiteSpace(course.ThumbnailUrl))
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Course thumbnail is required");
            }

            var activeSections = course.Sections
                .Where(s => !s.IsDeleted && s.IsActive)
                .ToList();

            if (!activeSections.Any())
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Course must contain at least one section");
            }

            var activeLessons = activeSections
                .SelectMany(s => s.Lessons)
                .Where(l => !l.IsDeleted && l.IsPublished)
                .ToList();

            if (!activeLessons.Any())
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Course must contain at least one published lesson");
            }

            var hasVideos = activeLessons
                .Any(l => l.Videos.Any(v => !v.IsDeleted));

            if (!hasVideos)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Course must contain at least one video");
            }

            course.Status = CourseStatus.Published;
            course.PublishedAt = DateTime.Now;
            course.UpdatedAt = DateTime.Now;
            course.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
               
                "Course published successfully");
        }
    }
}
