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

namespace ElearningPlatform.Application.Features.Lessons.Commands.BulkToggleLessonPreview
{
    public class BulkToggleLessonPreviewCommandHandler : IRequestHandler<BulkToggleLessonPreviewCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkToggleLessonPreviewCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(BulkToggleLessonPreviewCommand request, CancellationToken cancellationToken)
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
                    && !l.IsDeleted
                    && l.IsPublished 
                );

            var lessons = await lessonsQuery.ToListAsync(cancellationToken);

            if (!lessons.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No valid lessons found");

        
            var unauthorized = lessons.Any(l =>
                l.Section.Course.Instructor.UserId != userId);

            if (unauthorized)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Not allowed");

          
            var foundIds = lessons.Select(l => l.Id).ToHashSet();
            var skippedCount = request.LessonIds.Count(id => !foundIds.Contains(id));
     var updatedCount = await lessonsQuery.ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.IsPreview, request.IsPreview)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                .SetProperty(x => x.UpdatedBy, currentUserService.UserName),
                cancellationToken);

            var action = request.IsPreview ? "preview enabled" : "preview disabled";

            var message = skippedCount > 0
                ? $"{updatedCount} lessons updated, {skippedCount} skipped (not published or invalid)"
                : $"{updatedCount} lessons {action} successfully";

            return Result<string>.Success(message);

        }
    }
}
