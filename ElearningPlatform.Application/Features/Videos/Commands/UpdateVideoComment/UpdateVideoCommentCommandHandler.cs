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

namespace ElearningPlatform.Application.Features.Videos.Commands.UpdateVideoComment
{
    public class UpdateVideoCommentCommandHandler
    : IRequestHandler<UpdateVideoCommentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateVideoCommentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            UpdateVideoCommentCommand request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User not authenticated");
            var userId = currentUserService.UserId;

            var comment = await unitOfWork.videoComments.Query()
                .FirstOrDefaultAsync(c =>
                    c.Id == request.Id &&
                    !c.IsDeleted,
                    cancellationToken);

            if (comment is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Comment not found");

            if (comment.UserId != userId)
                return Result<string>.Failure(ResultStatus.Forbidden, "Only owner can edit comment");

            comment.Content = request.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.SaveAsync();

            return Result<string>.Success( "Comment updated");
        }
    }
}
