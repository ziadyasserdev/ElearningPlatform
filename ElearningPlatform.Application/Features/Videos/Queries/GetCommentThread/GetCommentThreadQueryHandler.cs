using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.GetCommentThread
{
    public class GetCommentThreadQueryHandler
    : IRequestHandler<GetCommentThreadQuery, Result<VideoCommentThreadDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCommentThreadQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<VideoCommentThreadDto>> Handle(
            GetCommentThreadQuery request,
            CancellationToken cancellationToken)
        {
            var comment = await unitOfWork.videoComments.Query()
                .Include(c => c.User)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(c =>
                    c.Id == request.Id && !c.IsDeleted,
                    cancellationToken);

            if (comment is null)
                return Result<VideoCommentThreadDto>.Failure(ResultStatus.NotFound, "Comment not found");

            VideoCommentThreadDto Map(VideoComment c)
            {
                return new VideoCommentThreadDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserName = c.User.UserName,
                    CreatedAt = c.CreatedAt,
                    Replies = c.Replies?
                        .Where(r => !r.IsDeleted)
                        .Select(Map)
                        .ToList()
                };
            }

            return Result<VideoCommentThreadDto>.Success(Map(comment));
        }
    }
}
