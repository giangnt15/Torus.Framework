namespace Torus.Framework.Domain.Events
{
    /// <summary>
    /// Defines an interface for asynchronously handling events of a specific type.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event to be handled. It must implement the <see cref="IEvent"/> interface.</typeparam>
    public interface IAsyncEventHandler<TEvent> where TEvent : IEvent
    {
        /// <summary>
        /// Asynchronously handles the specified event.
        /// </summary>
        /// <param name="event">The event to be handled.</param>
        /// <param name="cancellationToken">A token that can be used to signal cancellation of the operation (optional).</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}
