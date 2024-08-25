using BankingControlAPI.Interfaces;
using MediatR;
using System.Threading.Channels;

namespace BankingControlAPI.Services
{
    public sealed class DefaultBackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<CancellationToken, ValueTask>> _queue;
        private readonly Channel<INotification> _notificationQueue;

        public DefaultBackgroundTaskQueue(int capacity)
        {
            BoundedChannelOptions options = new(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };

            _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
            _notificationQueue = Channel.CreateBounded<INotification>(options);
        }

        public async ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> workItem)
        {
            ArgumentNullException.ThrowIfNull(workItem);

            await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
        {
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);

            return workItem;
        }

        public async Task QueueEventAsync(INotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification);

            await _notificationQueue.Writer.WriteAsync(notification);
        }

        public async Task<INotification> DequeueEventAsync(CancellationToken cancellationToken)
        {
            var workItem = await _notificationQueue.Reader.ReadAsync(cancellationToken);

            return workItem;
        }
    }
}
