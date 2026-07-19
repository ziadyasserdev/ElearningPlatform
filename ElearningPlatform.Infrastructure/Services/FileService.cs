using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Application.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
namespace ElearningPlatform.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly FileStorageSettings _settings;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(
            IOptions<FileStorageSettings> settings,
            IWebHostEnvironment webHostEnvironment)
        {
            _settings = settings.Value;
            _webHostEnvironment = webHostEnvironment;
        }

        public Task<Result<string>> UploadImageAsync(IFormFile file)
            => UploadAsync(
                file,
                _settings.ImageExtensionAllowed,
                _settings.ImageSizeInMB,
                _settings.ImagesFolder);

        public Task<Result<string>> UploadVideoAsync(IFormFile file)
            => UploadAsync(
                file,
                _settings.VideoExtensionAllowed,
                _settings.VideoSizeInMB,
                _settings.VideosFolder);

        public Task<Result<string>> UploadFileAsync(IFormFile file)
            => UploadAsync(
                file,
                _settings.FileExtensionAllowed,
                _settings.FileSizeInMB,
                _settings.FilesFolder);

        public async Task<Result<byte[]>> DownloadFileAsync(string url)
        {
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, url);

            if (!File.Exists(fullPath))
                return Result<byte[]>.Failure(ResultStatus.Failure, "File not found.");

            var bytes = await File.ReadAllBytesAsync(fullPath);

            return Result<byte[]>.Success(bytes);
        }

        public Result<string> Remove(string url)
        {
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, url);

            if (!File.Exists(fullPath))
                return Result<string>.Failure(ResultStatus.Failure, "File not found.");

            File.Delete(fullPath);

            return Result<string>.Success("File removed successfully.");
        }

        private string EnsureFolder(string folderName)
        {
            var path = Path.Combine(
                _webHostEnvironment.WebRootPath,
                _settings.UploadsFolder,
                folderName);

            Directory.CreateDirectory(path);

            return path;
        }

        private async Task<Result<string>> UploadAsync(
            IFormFile file,
            string[] allowedExtensions,
            int maxSizeInMB,
            string folderName)
        {
            var validationResult = ValidateFile(
                file,
                allowedExtensions,
                maxSizeInMB);

            if (!validationResult.IsSuccess)
            {
                return Result<string>.Failure(
                    validationResult.Status,
                    validationResult.Error);
            }

            var folderPath = EnsureFolder(folderName);

            var extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            var newFileName = $"{Guid.NewGuid()}{extension}";

            var fullPath = Path.Combine(folderPath, newFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"{_settings.UploadsFolder}/{folderName}/{newFileName}";

            return Result<string>.Success(url);
        }

        private Result<object> ValidateFile(
            IFormFile file,
            string[] allowedExtensions,
            int maxSizeInMB)
        {
            if (file is null || file.Length == 0)
            {
                return Result<object>.Failure(
                    ResultStatus.ValidationError,
                    "File cannot be null or empty.");
            }

            var extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            if (!allowedExtensions.Any(x =>
                x.Equals(extension, StringComparison.OrdinalIgnoreCase)))
            {
                return Result<object>.Failure(
                    ResultStatus.ValidationError,
                    $"Files with '{extension}' extension are not allowed.");
            }

            if (file.Length > maxSizeInMB * 1024 * 1024)
            {
                return Result<object>.Failure(
                    ResultStatus.ValidationError,
                    $"Maximum allowed file size is {maxSizeInMB} MB.");
            }

            return Result<object>.Success(null);
        }
    }
    //   public class FileService : IFileService
    //   {
    //       private readonly FileStorageSettings _settings;
    //       private readonly IWebHostEnvironment webHostEnvironment;

    //       public FileService(IOptions<FileStorageSettings> settings,IWebHostEnvironment webHostEnvironment)
    //       {
    //           _settings = settings.Value;
    //           this.webHostEnvironment = webHostEnvironment;
    //       }

    //       public async Task<Result<byte[]>> DownloadFileAsync(string url)
    //       {
    //           var fullPath = Path.Combine(webHostEnvironment.WebRootPath, url);

    //           if (!File.Exists(fullPath))
    //               return Result<byte[]>.Failure(ResultStatus.Failure, "File not found");

    //           var fileBytes = await File.ReadAllBytesAsync(fullPath);

    //           return Result<byte[]>.Success(fileBytes);
    //       }

    //       public Result<string> Remove(string url)
    //       {


    //           var fullPath = Path.Combine(webHostEnvironment.WebRootPath, url);

    //           if (!File.Exists(fullPath))
    //               return Result<string>.Failure(ResultStatus.Failure, $"File not found at path: {fullPath}");

    //           File.Delete(fullPath);
    //           return Result<string>.Success("File removed successfully");
    //       }

    //       public Task<Result<string>> UploadImageAsync(IFormFile file)
    //        => UploadAsync(file, new[] { ".jpg", ".png", ".jpeg" }, _settings.ImageSizeInMB, _settings.ImagesFolder);

    //       public Task<Result<string>> UploadVideoAsync(IFormFile file)
    // => UploadAsync(file, _settings.VideoExtensionAllowed, _settings.VideoSizeInMB, _settings.VideosFolder);

    //       private string EnsureFolder(string folderName)
    //       {
    //           string path = Path.Combine(webHostEnvironment.WebRootPath, _settings.UploadsFolder, folderName);
    //           Directory.CreateDirectory(path);
    //           return path;
    //       }
    //       private async Task<Result<string>> UploadAsync(
    //   IFormFile file,
    //   string[] extensionAllowed,
    //   int maxSizeInMB,
    //   string folderName)
    //       {
    //           var validationResult = ValidateFile(file, extensionAllowed, maxSizeInMB);

    //           if (!validationResult.IsSuccess)
    //               return Result<string>.Failure(
    //                   validationResult.Status,
    //                   validationResult.Error
    //               );

    //           var folderPath = EnsureFolder(folderName);

    //           string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
    //           string newName = $"{Guid.NewGuid()}{extension}";
    //           string fullPath = Path.Combine(folderPath, newName);

    //           using (var fileStream = new FileStream(fullPath, FileMode.Create))
    //           {
    //               await file.CopyToAsync(fileStream);
    //           }


    //           var Url = $"{_settings.UploadsFolder}/{folderName}/{newName}";
    //           return Result<string>.Success(Url);
    //       }







    //       private Result<object> ValidateFile(
    //IFormFile file,
    //string[] allowedExtensions,
    //int maxSizeInMB)
    //       {
    //           if (file == null || file.Length == 0)
    //               return Result<object>.Failure(
    //                   ResultStatus.ValidationError,
    //                   "File cannot be null or empty."
    //               );

    //           string extension = Path.GetExtension(file.FileName);
    //           if (allowedExtensions == null || !allowedExtensions.Any())
    //               return Result<object>.Failure(
    //                   ResultStatus.Failure,
    //                   "File extensions are not configured."
    //               );
    //           if (!allowedExtensions.Any(e =>
    //                   e.Equals(extension, StringComparison.OrdinalIgnoreCase)))
    //               return Result<object>.Failure(
    //                   ResultStatus.ValidationError,
    //                   $"Extension '{extension}' is not allowed."
    //               );

    //           if (file.Length > maxSizeInMB * 1024 * 1024)
    //               return Result<object>.Failure(
    //                   ResultStatus.ValidationError,
    //                   $"File size exceeds {maxSizeInMB}MB."
    //               );

    //           return Result<object>.Success(null);
    //       }





    //   }
}
