using MediatR;

namespace BankingControlAPI.Interfaces
{
    public interface IBackgroundTaskQueue
    {
        Task QueueEventAsync(INotification notification);
        Task<INotification> DequeueEventAsync(CancellationToken cancellationToken);
        ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> workItem);
        ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
    }
}
