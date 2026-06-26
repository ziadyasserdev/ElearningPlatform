using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Lessons.Dtos;
using ElearningPlatform.Application.Features.Videos.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Queries.GetInstructorLessonsBySection
{
    public class GetInstructorLessonsBySectionQueryHandler : IRequestHandler<GetInstructorLessonsBySectionQuery, Result<List<LessonInstructorDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        public GetInstructorLessonsBySectionQueryHandler(
       IUnitOfWork unitOfWork,
       ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<LessonInstructorDto>>> Handle(GetInstructorLessonsBySectionQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

         
            var isOwner = await unitOfWork.Sections.Query()
                .AnyAsync(s =>
                    s.Id == request.SectionId &&
                    s.Course.Instructor.UserId == userId,
                    cancellationToken);

            if (!isOwner)
                return Result<List<LessonInstructorDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "You are not allowed to access this section");

           
            var lessons = await unitOfWork.Lessons.Query()
                .Where(l =>
                    l.SectionId == request.SectionId 
                    )
                .OrderBy(l => l.OrderIndex)
                .Select(l => new LessonInstructorDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Description = l.Description,
                    Duration = l.Duration,
                    OrderIndex = l.OrderIndex,

                    IsPublished = l.IsPublished,
                    IsDeleted = l.IsDeleted,

                    Videos = l.Videos
                        .Where(v => !v.IsDeleted)
                        .Select(v => new VideoDto
                        {
                            Id = v.Id,
                            Url = v.FileUrl
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);

            return Result<List<LessonInstructorDto>>.Success(lessons);
        }
    }
}
