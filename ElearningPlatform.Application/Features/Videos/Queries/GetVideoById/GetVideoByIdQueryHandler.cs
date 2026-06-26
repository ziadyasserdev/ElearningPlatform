using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Videos.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.GetVideoById
{
    public class GetVideoByIdQueryHandler : IRequestHandler<GetVideoByIdQuery, Result<VideoDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetVideoByIdQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<VideoDetailsDto>> Handle(GetVideoByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var isInstructor = currentUserService.IsInRole("Instructor");

            var video = await unitOfWork.Videos.Query()
                .Where(v => v.Id == request.Id && !v.IsDeleted)
                .Select(v => new
                {
                    v.Id,
                    v.Title,
                    v.FileUrl,
                    v.Duration,
                    v.LessonId,
                    v.ProcessingStatus,

                   
                    LessonPublished = v.Lesson.IsPublished,
                    InstructorId = v.Lesson.Section.Course.Instructor.UserId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (video is null)
                return Result<VideoDetailsDto>.Failure(ResultStatus.NotFound, "Video not found");

            var isOwner = video.InstructorId == userId;

          
            if (video.ProcessingStatus != VideoProcessingStatus.Ready && !isOwner)
                return Result<VideoDetailsDto>.Failure(ResultStatus.Failure, "Video is still processing");

        
            if (!video.LessonPublished && !isOwner)
                return Result<VideoDetailsDto>.Failure(ResultStatus.Forbidden, "Video not accessible");

            var dto = new VideoDetailsDto
            {
                Id = video.Id,
                Title = video.Title,
                Url = video.FileUrl,
                Duration = video.Duration,
                LessonId = video.LessonId,
                IsPublished = video.LessonPublished
            };

            return Result<VideoDetailsDto>.Success(dto);
        }
    }
}
