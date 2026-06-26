using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.ReplyVideoComment
{
    public class ReplyVideoCommentCommandHandler
     : IRequestHandler<ReplyVideoCommentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ReplyVideoCommentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            ReplyVideoCommentCommand request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var userId = currentUserService.UserId;

            var parent = await unitOfWork.videoComments.Query()
                .FirstOrDefaultAsync(c =>
                    c.Id == request.CommentId &&
                    c.VideoId == request.VideoId &&
                    !c.IsDeleted,
                    cancellationToken);

            if (parent is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Parent comment not found");

            var reply = new VideoComment
            {
                VideoId = request.VideoId,
                ParentCommentId = request.CommentId,
                UserId = userId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await unitOfWork.videoComments.AddAsync(reply);
            await unitOfWork.SaveAsync();

            return Result<string>.Success( "Reply added");
        }
    }
    }
