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

namespace ElearningPlatform.Application.Features.Videos.Commands.DeleteVideoComment
{
    public class DeleteVideoCommentCommandHandler
     : IRequestHandler<DeleteVideoCommentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteVideoCommentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            DeleteVideoCommentCommand request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User not authenticated");
            var userId = currentUserService.UserId;

            var comment = await unitOfWork.videoComments.Query()
                .Include(c => c.Video)
                    .ThenInclude(v => v.Lesson)
                        .ThenInclude(l => l.Section)
                            .ThenInclude(s => s.Course)
                                .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(c =>
                    c.Id == request.Id && !c.IsDeleted,
                    cancellationToken);

            if (comment is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Comment not found");

            var isOwner = comment.UserId == userId;
            var isInstructor = comment.Video.Lesson.Section.Course.Instructor.UserId == userId;

            if (!isOwner && !isInstructor)
                return Result<string>.Failure(ResultStatus.Forbidden, "Not allowed");

            comment.IsDeleted = true;
            comment.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.SaveAsync();

            return Result<string>.Success( "Comment deleted");
        }
    }
}
