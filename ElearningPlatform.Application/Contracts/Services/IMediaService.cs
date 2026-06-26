using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Videos.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Contracts.Services
{
    public interface IMediaService
    {
        Task<Result<MediaDto>> UploadVideoAsync(IFormFile file);
        Task<Result<string>> DeleteVideoAsync(string fileName);
        Task<Result<FileDownloadDto>> DownloadVideoAsync(string fileName);
    }
}
