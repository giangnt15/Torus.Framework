using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Domain.Events;

namespace Torus.Framework.Domain.Entities
{
    public interface IAggregateRoot : IEntity
    {
        IEnumerable<IEvent> GetLocalEvents();
        void ClearLocalEvents();
        IEnumerable<IEvent> GetDistributedEvents();
        void ClearDistributedEvents();
    }

    public interface IAggregateRoot<TId> : IEntity<TId>, IAggregateRoot
    {

    }
}
