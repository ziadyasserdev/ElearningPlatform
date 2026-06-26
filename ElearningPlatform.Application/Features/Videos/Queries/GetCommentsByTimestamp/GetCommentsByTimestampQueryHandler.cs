using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Videos.Queries.GetVideoComments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.GetCommentsByTimestamp
{
    public class GetCommentsByTimestampQueryHandler
    : IRequestHandler<GetCommentsByTimestampQuery, Result<List<VideoCommentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCommentsByTimestampQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<VideoCommentDto>>> Handle(
            GetCommentsByTimestampQuery request,
            CancellationToken cancellationToken)
        {
            var query = unitOfWork.videoComments.Query()
                .Where(c => c.VideoId == request.VideoId && !c.IsDeleted);

            if (request.Timestamp.HasValue)
            {
                var t = request.Timestamp.Value;
                query = query.Where(c =>
                    c.TimestampSeconds.HasValue &&
                    c.TimestampSeconds.Value >= t - 5 &&
                    c.TimestampSeconds.Value <= t + 5);
            }

            var comments = await query
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new VideoCommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserName = c.User.UserName,
                    TimestampSeconds = c.TimestampSeconds,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<VideoCommentDto>>.Success(comments);
        }
    }
}
