using ElearningPlatform.Application.Features.Assignments.Commands.CloseExpiredAssignments;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class AssignmentDeadlineWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AssignmentDeadlineWorker> _logger;

    public AssignmentDeadlineWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<AssignmentDeadlineWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(
            TimeSpan.FromMinutes(1));

        _logger.LogInformation(
            "Assignment deadline worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CloseExpiredAssignmentsAsync(
                    stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "Assignment deadline worker is stopping.");

                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while closing expired assignments.");
            }

            try
            {
                await timer.WaitForNextTickAsync(
                    stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }

        _logger.LogInformation(
            "Assignment deadline worker stopped.");
    }


    private async Task CloseExpiredAssignmentsAsync(
        CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider
            .GetRequiredService<IMediator>();

        var result = await mediator.Send(
            new CloseExpiredAssignmentsCommand(),
            cancellationToken);

        if (result.IsSuccess)
        {
            _logger.LogInformation(
                "Assignment cleanup completed. " +
                "{Count} assignments were closed.",
                result.Value);
        }
        else
        {
            _logger.LogWarning(
                "Assignment cleanup failed: {Message}",
                result.Message);
        }
    }
}