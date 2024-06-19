using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Domain.Events;

namespace Torus.Framework.Domain.Entities
{
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        public int Version { get; internal set; }

        private readonly ICollection<IEvent> _localEvents = new List<IEvent>();
        private readonly ICollection<IEvent> _distributedEvents = new List<IEvent>();
        public void ClearDistributedEvents() => _distributedEvents?.Clear();

        public void ClearLocalEvents() => _localEvents?.Clear();

        public IEnumerable<IEvent> GetDistributedEvents() => _distributedEvents?.ToList();

        public IEnumerable<IEvent> GetLocalEvents() => _localEvents?.ToList();

        /// <summary>
        /// Method to actually apply event to this aggregate
        /// Must be pure, contain no side effects.
        /// </summary>
        /// <param name="event">The event to be applied</param>
        protected abstract void When(IEvent @event);

        protected abstract void EnsureValidState();

        /// <summary>
        /// Apply event to this aggregate
        /// </summary>
        /// <param name="event">The event to be applied</param>
        protected virtual void Apply(IEvent @event, bool addToLocal = false)
        {
            ArgumentNullException.ThrowIfNull(nameof(@event));
            @event.Version = Version + 1;
            When(@event);
            EnsureValidState();
            AddDistributedEvent(@event);
            if (addToLocal)
            {
                _localEvents.Add(@event);
            }
        }

        protected virtual void AddLocalEvent(IEvent @event)
        {
            _localEvents.Add(@event);
        }

        protected virtual void AddDistributedEvent(IEvent @event)
        {
            _distributedEvents.Add(@event);
        }
    }

    public abstract class AggregateRoot<TId> : AggregateRoot, IAggregateRoot<TId>
    {
        public TId Id { get; set; }

        protected override void Apply(IEvent @event, bool addToLocal = false)
        {
            base.Apply(@event, addToLocal);
            @event.SourceId = Id;
        }
    }
}
