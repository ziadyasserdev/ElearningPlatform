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

namespace ElearningPlatform.Application.Features.Videos.Commands.CreateVideoComment
{
    public class CreateVideoCommentCommandHandler
     : IRequestHandler<CreateVideoCommentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateVideoCommentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            CreateVideoCommentCommand request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var userId = currentUserService.UserId;

            var video = await unitOfWork.Videos.Query()
                .FirstOrDefaultAsync(v => v.Id == request.VideoId && !v.IsDeleted,
                    cancellationToken);

            if (video is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Video not found");

            var comment = new VideoComment
            {
                VideoId = request.VideoId,
                UserId = userId,
                Content = request.Content,
                TimestampSeconds = request.TimestampSeconds,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            await unitOfWork.videoComments.AddAsync(comment);
            await unitOfWork.SaveAsync();

            return Result<string>.Success( "Comment added successfully");
        }
    }
    }
