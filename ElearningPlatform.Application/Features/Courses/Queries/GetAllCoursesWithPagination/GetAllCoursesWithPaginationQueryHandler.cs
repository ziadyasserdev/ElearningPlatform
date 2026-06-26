using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Courses.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Queries.GetAllCoursesWithPagination
{
    public class GetAllCoursesWithPaginationQueryHandler : IRequestHandler<GetAllCoursesWithPaginationQuery, Result<PaginatedResult<object>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllCoursesWithPaginationQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<PaginatedResult<object>>> Handle(GetAllCoursesWithPaginationQuery request, CancellationToken cancellationToken)
        {
           if(currentUserService.IsInRole("Admin"))
            {
                var courses = await unitOfWork.Courses.Query()
                     .Skip((request.PageNumber - 1) * request.PageSize)
                     .Take(request.PageSize)
                   .Select(x => new CourseAdminDto
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
                       PublishedAt = x.PublishedAt,
                       Price = x.Price,
                       TotalDurationInMinutes = x.TotalDurationInMinutes,
                       TotalLessons = x.TotalLessons,
                       TotalReviews = x.TotalReviews,
                       UpdatedAt = x.UpdatedAt
                   }).ToListAsync(cancellationToken);
                return Result<PaginatedResult<object>>.Success(new PaginatedResult<object>(courses, request.PageNumber, request.PageSize, courses.Count));

            }
            else
            {
                var course = await unitOfWork.Courses.Query()
                     .Where(x => x.IsActive && !x.IsDeleted
                     && x.Status == Domain.Enums.CourseStatus.Published)
                     .Skip((request.PageNumber - 1) * request.PageSize)
                     .Take(request.PageSize)
                     .Select(x => new CourseUserDto
                     {
                            Id = x.Id,
                            Title = x.Title,
                            
                           
                            ShortDescription = x.ShortDescription,
                          
                            TotalStudents = x.TotalStudents,
                            AverageRating = x.AverageRating,
                            CategoryName = x.Category.Name,
                            InstructorName = x.Instructor.User.FullName,
                          
                            DiscountPrice = x.DiscountPrice,
                          
                          Language = x.Language,
                          Level = x.Level.ToString(),
                          ThumbnailUrl = x.ThumbnailUrl!,
                            Price = x.Price,
                          

                     }).ToListAsync(cancellationToken);
                return Result<PaginatedResult<object>>.Success(new PaginatedResult<object>(course, request.PageNumber, request.PageSize, course.Count));
            }
        }
    }
}
