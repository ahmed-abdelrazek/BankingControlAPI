using BankingControlAPI.Interfaces;
using MediatR;

namespace BankingControlAPI.HostedServices
{
    public sealed class QueuedHostedService(IBackgroundTaskQueue taskQueue, ILogger<QueuedHostedService> logger, IServiceProvider services) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("""{Name} is running.""", nameof(QueuedHostedService));

            return ProcessTaskQueueAsync(stoppingToken);
        }

        private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var @event = await taskQueue.DequeueEventAsync(stoppingToken);
                    if (@event is INotification)
                    {
                        using var scope = services.CreateScope();
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        await mediator.Publish(@event, stoppingToken);
                    }
                    else
                    {
                        var workItem = await taskQueue.DequeueAsync(stoppingToken);
                        await workItem(stoppingToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Prevent throwing if stoppingToken was signaled
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred executing task work item.");
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("""{Name} is stopping.""", nameof(QueuedHostedService));

            await base.StopAsync(stoppingToken);
        }
    }
}
