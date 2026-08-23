using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Coupons.Commands.DeactivateExpiredCoupons;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Infrastructure.BackgroundServices
{
    public class CouponExpirationWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CouponExpirationWorker> _logger;

        public CouponExpirationWorker(
            IServiceScopeFactory scopeFactory,
            ILogger<CouponExpirationWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope =
                        _scopeFactory.CreateScope();

                    var mediator =
                        scope.ServiceProvider
                            .GetRequiredService<IMediator>();

                    var result =
                        await mediator.Send(
                            new DeactivateExpiredCouponsCommand(),
                            stoppingToken);

                    _logger.LogInformation(
                        "Deactivated {Count} expired coupons.",
                        result.Value);
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error while deactivating expired coupons.");
                }

                await Task.Delay(
                    TimeSpan.FromMinutes(1),
                    stoppingToken);
            }
        }
    }
}
