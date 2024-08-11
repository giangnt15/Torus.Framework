using Microsoft.Extensions.DependencyInjection;

namespace Torus.Framework.Domain.Events
{
    public class AsyncEventDispatcher(IServiceProvider serviceProvider) : IAsyncEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        public async Task DispatchAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
        {
            var handlers = _serviceProvider.GetServices<IAsyncEventHandler<TEvent>>();
            if (handlers != null && handlers.Any())
            {
                foreach (var handler in handlers)
                {
                    await handler.HandleAsync(@event, cancellationToken);
                }
            }
        }
    }
}
