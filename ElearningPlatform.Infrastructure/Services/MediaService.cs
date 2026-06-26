using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Application.Features.Videos.Dtos;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Infrastructure.Services
{
    public class MediaService : IMediaService
    {
        private readonly IWebHostEnvironment _env;

        private const string VideosFolder = "uploads/videos";

        public MediaService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<Result<MediaDto>> UploadVideoAsync(IFormFile file)
        {
            //if (file == null || file.Length == 0)
            //    return Result<MediaDto>.Failure(ResultStatus.Failure, "File is required");


            //var allowedExt = new[] { ".mp4", ".mov", ".avi", ".mkv" };

            //var ext = Path.GetExtension(file.FileName).ToLower();

            //if (!allowedExt.Contains(ext))
            //    return Result<MediaDto>.Failure(ResultStatus.Failure, "Invalid video format");


            //if (file.Length > 500 * 1024 * 1024)
            //    return Result<MediaDto>.Failure(ResultStatus.Failure, "Max size is 500MB");


            //var folderPath = Path.Combine(_env.WebRootPath, VideosFolder);

            //if (!Directory.Exists(folderPath))
            //    Directory.CreateDirectory(folderPath);


            //var fileName = $"{Guid.NewGuid()}{ext}";
            //var fullPath = Path.Combine(folderPath, fileName);


            //using (var stream = new FileStream(fullPath, FileMode.Create))
            //{
            //    await file.CopyToAsync(stream);
            //}


            //return Result<MediaDto>.Success(new MediaDto
            //{
            //    OriginalName = file.FileName,
            //    FakeName = fileName,
            //    ContentType = file.ContentType,
            //    Size = file.Length,
            //    Url = $"/{VideosFolder}/{fileName}"
            //});
            //if (file == null || file.Length == 0)
            //    return Result<MediaDto>.Failure(ResultStatus.Failure, "File is required");

            //var allowedExt = new[] { ".mp4", ".mov", ".avi", ".mkv" };
            //var ext = Path.GetExtension(file.FileName).ToLower();

            //if (!allowedExt.Contains(ext))
            //    return Result<MediaDto>.Failure(ResultStatus.Failure, "Invalid video format");

            //if (file.Length > 500 * 1024 * 1024)
            //    return Result<MediaDto>.Failure(ResultStatus.Failure, "Max size is 500MB");


            //var folderPath = Path.Combine(_env.WebRootPath, VideosFolder);

            //if (!Directory.Exists(folderPath))
            //    Directory.CreateDirectory(folderPath);

            //var fileName = $"{Guid.NewGuid()}{ext}";
            //var fullPath = Path.Combine(folderPath, fileName);

            //using (var stream = new FileStream(fullPath, FileMode.Create))
            //{
            //    await file.CopyToAsync(stream);
            //}

            //return Result<MediaDto>.Success(new MediaDto
            //{
            //    OriginalName = file.FileName,
            //    FakeName = fileName,
            //    ContentType = file.ContentType,
            //    Size = file.Length,
            //    Url = $"{fileName}" 
            //});
            if (file == null || file.Length == 0)
                return Result<MediaDto>.Failure(ResultStatus.Failure, "File is required");

            var allowedExt = new[] { ".mp4", ".mov", ".avi", ".mkv" };
            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExt.Contains(ext))
                return Result<MediaDto>.Failure(ResultStatus.Failure, "Invalid video format");

            if (file.Length > 500 * 1024 * 1024)
                return Result<MediaDto>.Failure(ResultStatus.Failure, "Max size is 500MB");

            var folderPath = Path.Combine(_env.WebRootPath, VideosFolder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Result<MediaDto>.Success(new MediaDto
            {
                OriginalName = file.FileName,
                FakeName = fileName,
                ContentType = file.ContentType,
                Size = file.Length,

                
                Url = fileName
            });
        }

        public Task<Result<string>> DeleteVideoAsync(string fileUrl)
        {
            var fileName = Path.GetFileName(fileUrl); 

            var path = Path.Combine(_env.WebRootPath, VideosFolder, fileName);

            if (!File.Exists(path))
                return Task.FromResult(Result<string>.Failure(ResultStatus.NotFound, "File not found"));

            File.Delete(path);

            return Task.FromResult(Result<string>.Success("Deleted successfully"));
        }

        public async Task<Result<FileDownloadDto>> DownloadVideoAsync(string fileName)
        {
            var fullPath = Path.Combine(
                _env.WebRootPath,
                "uploads/videos",
                fileName
            );

            if (!File.Exists(fullPath))
                return Result<FileDownloadDto>.Failure(ResultStatus.NotFound, "File not found");

            var bytes = await File.ReadAllBytesAsync(fullPath);

            var ext = Path.GetExtension(fileName);

            var contentType = ext switch
            {
                ".mp4" => "video/mp4",
                ".avi" => "video/x-msvideo",
                ".mov" => "video/quicktime",
                _ => "application/octet-stream"
            };

            return Result<FileDownloadDto>.Success(new FileDownloadDto
            {
                FileBytes = bytes,
                ContentType = contentType,
                FileName = fileName
            });
        }
    }
}
