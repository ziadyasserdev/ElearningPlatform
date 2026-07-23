using ElearningPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Contracts.Services
{
    public interface ICertificateGenerator
    {
        Task<string> GenerateAsync(
            Certificate certificate,
            CancellationToken cancellationToken = default);
    }
}
