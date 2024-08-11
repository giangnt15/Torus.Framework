using Torus.Framework.Domain.Entities;
using Torus.Framework.Domain.Events;

namespace Torus.Framework.Domain.EventSourcing
{
    public abstract class EventSourcedAggregateRoot : AggregateRoot, IEventSourcedAggregateRoot
    {
        public virtual void Load(IEnumerable<IEvent> events)
        {
            foreach (var evt in events)
            {
                Load(evt);
            }
        }

        public virtual void Load(IEvent @event)
        {
            When(@event);
        }
    }

    public abstract class EventSourcedAggregateRoot<TId> : AggregateRoot<TId>, IEventSourcedAggregateRoot<TId>
    {
        public virtual void Load(IEnumerable<IEvent> events)
        {
            foreach (var evt in events)
            {
                Load(evt);
            }
        }

        public virtual void Load(IEvent @event)
        {
            When(@event);
        }
    }
}
