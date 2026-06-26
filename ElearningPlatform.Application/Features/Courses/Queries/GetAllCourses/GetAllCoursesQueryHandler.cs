using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Categories.Dtos;
using ElearningPlatform.Application.Features.Courses.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Queries.GetAllCourses
{
    public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, Result<List<object>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllCoursesQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<object>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {

            var query = unitOfWork.Courses.Query();
            if (currentUserService.IsInRole("Admin") ||
               currentUserService.IsInRole("Instructor"))
            {
                var courses = await query.Select(x => new CourseAdminDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,

                    Slug = x.Slug,

                    IsDeleted = x.IsDeleted,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    ShortDescription = x.ShortDescription,
                    Status = x.Status.ToString(),
                    TotalStudents = x.TotalStudents,
                    AverageRating = x.AverageRating,
                    CategoryName = x.Category.Name,
                    InstructorName = x.Instructor.User.FullName,
                    DiscountEndDate = x.DiscountEndDate,
                    DiscountPrice = x.DiscountPrice,
                    IsFeatured = x.IsFeatured,
                    PublishedAt = x.CreatedAt,
                    Price = x.Price,
                    TotalDurationInMinutes = x.TotalDurationInMinutes,
                    TotalLessons = x.TotalLessons,
                    TotalReviews = x.TotalReviews,
                    UpdatedAt = x.UpdatedAt


                }).ToListAsync(cancellationToken);

                return Result<List<object>>.Success(courses.Cast<object>().ToList());
            }
            else
            {
                query = query.Where(x => x.IsActive && !x.IsDeleted
                && x.Status == CourseStatus.Published);
                var courses = await query.Select(x => new CourseUserDto
                {
                    Id = x.Id,
                    Title = x.Title,





                    ShortDescription = x.ShortDescription,

                    TotalStudents = x.TotalStudents,
                    AverageRating = x.AverageRating,
                    CategoryName = x.Category.Name,
                    InstructorName = x.Instructor.User.FullName,

                    DiscountPrice = x.DiscountPrice,

                    Price = x.Price,
                    Language = x.Language,
                    Level = x.Level.ToString(),
                    ThumbnailUrl = x.ThumbnailUrl!



                }).ToListAsync(cancellationToken);
                return Result<List<object>>.Success(courses.Cast<object>().ToList());
            }
        }
    }
}
