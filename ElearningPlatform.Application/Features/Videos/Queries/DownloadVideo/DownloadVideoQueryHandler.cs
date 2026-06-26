using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Videos.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.Threading.Tasks;
using ElearningPlatform.Application.Contracts.Services;

namespace ElearningPlatform.Application.Features.Videos.Queries.DownloadVideo
{
    public class DownloadVideoQueryHandler : IRequestHandler<DownloadVideoQuery, Result<FileDownloadDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediaService mediaService;

        public DownloadVideoQueryHandler(IUnitOfWork unitOfWork,IMediaService mediaService)
        {
            _unitOfWork = unitOfWork;
            this.mediaService = mediaService;
        }

        public async Task<Result<FileDownloadDto>> Handle(DownloadVideoQuery request, CancellationToken cancellationToken)
        {
            var lesson = await _unitOfWork.Lessons.Query()
                .AnyAsync(x => x.Id == request.LessonId && !x.IsDeleted, cancellationToken);
            if(!lesson)
                return Result<FileDownloadDto>.Failure(ResultStatus.NotFound, "Lesson not found");
            var video = await _unitOfWork.Videos.Query()
                  .FirstOrDefaultAsync(x =>
                      x.Id == request.VideoId &&
                      x.LessonId == request.LessonId &&
                      !x.IsDeleted,
                      cancellationToken);

            if (video == null)
                return Result<FileDownloadDto>.Failure(ResultStatus.NotFound, "Video not found");
          var result = await mediaService.DownloadVideoAsync(video.FileUrl);
            if (!result.IsSuccess)
                return Result<FileDownloadDto>.Failure(result.Status, result.Message);
            var file = result.Value!;
            return Result<FileDownloadDto>.Success(new FileDownloadDto
            {
                ContentType = file.ContentType,
                FileName = file.FileName,
                FileBytes = file.FileBytes
            });

        }
    }
}
