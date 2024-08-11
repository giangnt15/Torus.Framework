using Torus.Framework.Domain.Entities;
using Torus.Framework.Domain.Events;

namespace Torus.Framework.Domain.EventSourcing
{
    public interface IEventSourcedAggregateRoot : IAggregateRoot
    {
        void Load(IEnumerable<IEvent> events);
        void Load(IEvent @event);
    }

    public interface IEventSourcedAggregateRoot<TId> : IAggregateRoot<TId>, IEventSourcedAggregateRoot
    {
    }

}
