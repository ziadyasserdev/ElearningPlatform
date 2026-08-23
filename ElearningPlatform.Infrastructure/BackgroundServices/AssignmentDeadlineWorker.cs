using ElearningPlatform.Application.Features.Assignments.Commands.CloseExpiredAssignments;
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
    public class AssignmentDeadlineWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AssignmentDeadlineWorker> _logger;
        public AssignmentDeadlineWorker(IServiceScopeFactory serviceScope,
            ILogger<AssignmentDeadlineWorker> _logger)
        {
            this._scopeFactory = serviceScope;
            this._logger = _logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope =
                    _scopeFactory.CreateScope();

                var mediator =
                    scope.ServiceProvider
                        .GetRequiredService<IMediator>();

                var result = await mediator.Send(
                    new CloseExpiredAssignmentsCommand(),
                    stoppingToken);

                if (result.IsSuccess)
                {
                    _logger.LogInformation(
                        "Assignment cleanup completed. " +
                        "{Count} assignments were closed.",
                        result.Value);
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "Assignment deadline worker is stopping.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while closing expired assignments.");
            }

            await Task.Delay(
                TimeSpan.FromMinutes(1),
                stoppingToken);
        }
    }
}
