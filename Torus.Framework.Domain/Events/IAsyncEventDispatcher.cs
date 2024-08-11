using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Domain.Events
{
    /// <summary>
    /// Defines an interface for asynchronously dispatching events.
    /// </summary>
    public interface IAsyncEventDispatcher
    {
        /// <summary>
        /// Asynchronously dispatches the specified event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to be dispatched. It must implement the <see cref="IEvent"/> interface.</typeparam>
        /// <param name="event">The event to be dispatched.</param>
        /// <param name="cancellationToken">A token that can be used to signal cancellation of the operation (optional).</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DispatchAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;
    }
}
