using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.FeatureCourse
{
    public class FeatureCourseCommandHandler
         : IRequestHandler<FeatureCourseCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public FeatureCourseCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(
     FeatureCourseCommand request,
     CancellationToken cancellationToken)
        {
            var course = await unitOfWork.Courses.Query()
                .FirstOrDefaultAsync(
                    c => c.Id == request.CourseId &&
                         !c.IsDeleted,
                    cancellationToken);

            if (course is null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Course not found");
            }

            if (course.Status != CourseStatus.Published)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Only published courses can be featured");
            }

            if (course.IsFeatured)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Course is already featured");
            }

            var featuredCount = await unitOfWork.Courses.Query()
                .CountAsync(c => c.IsFeatured && !c.IsDeleted, cancellationToken);

            if (featuredCount >= 10)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Maximum featured courses limit reached");
            }

            course.IsFeatured = true;
           // course.FeaturedAt = DateTime.Now;
            course.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.SaveAsync();

            return Result<int>.Success(
                course.Id,
                "Course marked as featured");
        }
    }
}
