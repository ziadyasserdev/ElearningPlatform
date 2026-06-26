using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.GetVideoComments
{
    public class GetVideoCommentsQueryHandler
     : IRequestHandler<GetVideoCommentsQuery, Result<List<VideoCommentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetVideoCommentsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<VideoCommentDto>>> Handle(
            GetVideoCommentsQuery request,
            CancellationToken cancellationToken)
        {
            var comments = await unitOfWork.videoComments.Query()
                .Where(c => c.VideoId == request.VideoId && !c.IsDeleted && c.ParentCommentId == null)
                .Include(c => c.User)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.User)
                .ToListAsync(cancellationToken);

            var result = comments.Select(c => new VideoCommentDto
            {
                Id = c.Id,
                Content = c.Content,
                UserName = c.User.UserName,
                TimestampSeconds = c.TimestampSeconds,
                CreatedAt = c.CreatedAt,

                Replies = c.Replies
                    .Where(r => !r.IsDeleted)
                    .Select(r => new VideoCommentDto
                    {
                        Id = r.Id,
                        Content = r.Content,
                        UserName = r.User.UserName,
                        TimestampSeconds = r.TimestampSeconds,
                        CreatedAt = r.CreatedAt
                    }).ToList()
            }).ToList();

            return Result<List<VideoCommentDto>>.Success(result);
        }
    }
}
