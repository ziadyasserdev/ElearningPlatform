using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Infrastructure.Services
{
    public class VideoProcessingService : IVideoProcessingService
    {
        private readonly IUnitOfWork unitOfWork;

        public VideoProcessingService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task ProcessVideoAsync(int videoId)
        {
            var video = await unitOfWork.Videos.Query()
                .Include(v => v.Lesson)
                .FirstOrDefaultAsync(v => v.Id == videoId);

            if (video is null)
                return;

            try
            {
              
                video.ProcessingStatus = VideoProcessingStatus.Processing;
                await unitOfWork.SaveAsync();

        
                await Task.Delay(3000);

              
                var duration = new Random().Next(60, 600);

              
                video.Duration = duration;
                video.ProcessingStatus = VideoProcessingStatus.Ready;

              
                video.Lesson.Duration += duration;

                video.UpdatedAt = DateTime.Now;

                await unitOfWork.SaveAsync();
            }
            catch
            {
                video.ProcessingStatus = VideoProcessingStatus.Failed;
                await unitOfWork.SaveAsync();
            }
        }
    }
}
