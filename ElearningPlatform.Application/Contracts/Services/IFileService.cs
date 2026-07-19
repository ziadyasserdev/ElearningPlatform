using ElearningPlatform.Application.Common.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Contracts.Services
{
    public interface IFileService
    {
        Task<Result<string>> UploadImageAsync(IFormFile file);
        Task<Result<string>> UploadVideoAsync(IFormFile file);
        Task<Result<string>> UploadFileAsync(IFormFile file);
        Result<string> Remove(string url);
        Task<Result<byte[]>> DownloadFileAsync(string url);
    }
}
