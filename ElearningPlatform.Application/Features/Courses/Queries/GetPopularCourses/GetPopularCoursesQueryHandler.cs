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

namespace ElearningPlatform.Application.Features.Courses.Queries.GetPopularCourses
{
    public class GetPopularCoursesQueryHandler
         : IRequestHandler<GetPopularCoursesQuery, Result<List<CourseUserDtoo>>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork unitOfWork;

        public GetPopularCoursesQueryHandler(IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            this._httpContextAccessor = httpContextAccessor;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<CourseUserDtoo>>> Handle(
            GetPopularCoursesQuery request,
            CancellationToken cancellationToken)
        {
            var courses = await unitOfWork.Courses.Query()
                .Where(x =>
                    x.IsActive &&
                    !x.IsDeleted &&
                    x.Status == CourseStatus.Published)
                .OrderByDescending(x => x.TotalStudents)
                .ThenByDescending(x => x.AverageRating)
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
