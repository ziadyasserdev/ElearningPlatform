using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Lessons.Dtos;
using ElearningPlatform.Application.Features.Videos.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Queries.GetLessonsBySection
{
    public class GetLessonsBySectionQueryHandler : IRequestHandler<GetLessonsBySectionQuery, Result<List<LessonForSectionDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetLessonsBySectionQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<LessonForSectionDto>>> Handle(GetLessonsBySectionQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

         
            var isInstructor = currentUserService.IsInRole("Instructor");

            var isOwner = await unitOfWork.Sections.Query()
                .AnyAsync(s =>
                    s.Id == request.SectionId &&
                    s.Course.Instructor.UserId == userId,
                    cancellationToken);

            var canSeeAll = isInstructor && isOwner;

            var query = unitOfWork.Lessons.Query()
                .Where(l => l.SectionId == request.SectionId && !l.IsDeleted 
                && l.IsPublished);

            if (!canSeeAll)
            {
                query = query.Where(l => l.IsPublished);
            }

            var lessons = await query
                .OrderBy(l => l.OrderIndex)
                .Select(l => new LessonForSectionDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Description = l.Description,
                    Duration = l.Duration,
                    OrderIndex = l.OrderIndex,

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

            return Result<List<LessonForSectionDto>>.Success(lessons);
        }
    }
}
