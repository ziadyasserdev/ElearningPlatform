using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Commands.BulkTogglePublishLessons
{
    public class BulkTogglePublishLessonsCommandHandler : IRequestHandler<BulkTogglePublishLessonsCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkTogglePublishLessonsCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(BulkTogglePublishLessonsCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            if (request.LessonIds == null || !request.LessonIds.Any())
                return Result<string>.Failure(ResultStatus.Failure, "No lessons provided");

            var lessonsQuery = unitOfWork.Lessons.Query()
                .Include(l => l.Section)
                .ThenInclude(s => s.Course)
                .ThenInclude(c => c.Instructor)
                .Where(l =>
                    request.LessonIds.Contains(l.Id)
                    && !l.IsDeleted);

            var lessons = await lessonsQuery.ToListAsync(cancellationToken);

            if (!lessons.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No lessons found");

            if (lessons.Any(l => l.Section.Course.Instructor.UserId != userId))
                return Result<string>.Failure(ResultStatus.Unauthorized, "Not allowed");

            var updatedCount = await lessonsQuery.ExecuteUpdateAsync(s => s
                .SetProperty(x => x.IsPublished, request.IsPublished)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                .SetProperty(x => x.UpdatedBy, currentUserService.UserName),
                cancellationToken);

            var action = request.IsPublished ? "published" : "unpublished";

            return Result<string>.Success($"{updatedCount} lessons {action} successfully");
        }
    }
}
