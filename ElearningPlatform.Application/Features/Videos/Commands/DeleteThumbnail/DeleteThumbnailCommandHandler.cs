using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.DeleteThumbnail
{
    public class DeleteThumbnailCommandHandler : IRequestHandler<DeleteThumbnailCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileService _fileService;

        public DeleteThumbnailCommandHandler(IUnitOfWork unitOfWork
            , ICurrentUserService currentUserService
            , IFileService fileService)
        {
            this._unitOfWork = unitOfWork;
            this._currentUserService = currentUserService;
            this._fileService = fileService;
        }
        public  async Task<Result<int>> Handle(DeleteThumbnailCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var videoData = await _unitOfWork.Videos.Query()
                .Where(v => v.Id == request.VideoId && !v.IsDeleted)
                .Select(v => new
                {
                    Video = v,
                    InstructorId = v.Lesson.Section.Course.Instructor.UserId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (videoData == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Video not found");

            if (videoData.InstructorId != userId)
                return Result<int>.Failure(ResultStatus.Forbidden, "Not allowed");

            var video = videoData.Video;

            if (string.IsNullOrEmpty(video.ThumbnailUrl))
                return Result<int>.Failure(ResultStatus.NotFound, "Thumbnail does not exist");

          
            var deleteResult =  _fileService.Remove(video.ThumbnailUrl);

            if (!deleteResult.IsSuccess)
                return Result<int>.Failure(deleteResult.Status, deleteResult.Message ?? "Delete failed");

         
            video.ThumbnailUrl = null;
            video.UpdatedAt = DateTime.UtcNow;
            video.UpdatedBy = _currentUserService.UserName;

            await _unitOfWork.SaveAsync();

            return Result<int>.Success(video.Id, "Thumbnail deleted successfully");
        }
    }
}
