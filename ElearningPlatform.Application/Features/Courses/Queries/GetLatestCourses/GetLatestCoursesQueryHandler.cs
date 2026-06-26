using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Courses.Dtos;
using ElearningPlatform.Application.Features.Courses.Queries.GetFeaturedCourses;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Queries.GetLatestCourses
{
    public class GetLatestCoursesQueryHandler
      : IRequestHandler<GetLatestCoursesQuery, Result<List<CourseUserDtoo>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetLatestCoursesQueryHandler(IUnitOfWork unitOfWork,IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<CourseUserDtoo>>> Handle(
            GetLatestCoursesQuery request,
            CancellationToken cancellationToken)
        {
            var courses = await unitOfWork.Courses.Query()
                .Where(x =>
                    x.IsActive &&
                    !x.IsDeleted &&
                    x.Status == CourseStatus.Published)
                .OrderByDescending(x => x.PublishedAt)
                .Take(10)
                .Select(x => new CourseUserDtoo
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.ShortDescription,

                    Price = x.Price,
                    DiscountPrice = x.DiscountPrice,

                    AverageRating = (double)x.AverageRating,

                    TotalStudents = x.TotalStudents,
                    TotalLessons = x.TotalLessons,
                    TotalDurationInMinutes = x.TotalDurationInMinutes,

                 ThumbnailUrl = $"{_httpContextAccessor.HttpContext!.Request.Scheme}" +
                    $"://{_httpContextAccessor.HttpContext!.Request.Host}" +
                    $"/{x.ThumbnailUrl}",

                    CategoryName = x.Category.Name,
                    InstructorName = x.Instructor.User.FullName,

                    Language = x.Language,
                    Level = x.Level.ToString(),

                    IsFeatured = x.IsFeatured
                })
                .ToListAsync(cancellationToken);

            return Result<List<CourseUserDtoo>>.Success(courses);
        }
    }
}
