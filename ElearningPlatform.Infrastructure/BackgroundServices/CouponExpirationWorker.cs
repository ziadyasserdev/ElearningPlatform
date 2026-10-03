using ElearningPlatform.Application.Features.Coupons.Commands.DeactivateExpiredCoupons;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
        using var timer =
            new PeriodicTimer(TimeSpan.FromDays(1));

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DeactivateExpiredCouponsAsync(
                    stoppingToken);
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
                    "Error occurred while deactivating expired coupons.");
            }

            await timer.WaitForNextTickAsync(
                stoppingToken);
        }

        _logger.LogInformation(
            "Coupon expiration worker is stopping.");
    }

    private async Task DeactivateExpiredCouponsAsync(
        CancellationToken cancellationToken)
    {
        using var scope =
            _scopeFactory.CreateScope();

        var mediator =
            scope.ServiceProvider
                .GetRequiredService<IMediator>();

        var result = await mediator.Send(
            new DeactivateExpiredCouponsCommand(),
            cancellationToken);

        _logger.LogInformation(
            "Expired coupon processing completed: {Message}",
            result.Message);
    }
}