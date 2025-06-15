using EventTracker.Application;
using EventTracker.Infrastructure;

namespace EventTracker;

public class Worker(ILogger<Worker> logger,
    IServiceScopeFactory serviceScopeFactory,
    WorkerConfiguration workerConfiguration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        logger.LogInformation("Application started");

        while (!ct.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var workerService = scope.ServiceProvider.GetRequiredService<WorkerService>();
                await workerService.TrackEvents(ct);
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Unhandled exception");
            }
            await Task.Delay(workerConfiguration.Interval, ct);
        }

        logger.LogInformation("Application finished");
    }
}