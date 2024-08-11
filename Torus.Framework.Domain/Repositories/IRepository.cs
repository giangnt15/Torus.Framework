using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Domain.Entities;
using Torus.Framework.Domain.EventSourcing;

namespace Torus.Framework.Domain.Repositories
{
    public interface IRepository
    {

    }

    public interface IRepository<TKey, TEntity> : IRepository where TEntity : class, IEntity<TKey>
    {
        Task<TEntity> LoadAsync(TKey key, CancellationToken cancellationToken = default);
        Task SaveAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<bool> ExistAsync(TKey key, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    }

    public interface IEventSourcedRepository<TKey, TAggregateRoot> : IRepository<TKey, TAggregateRoot> where TAggregateRoot : EventSourcedAggregateRoot<TKey>
    {
        
    }

    public interface IRepository<TEntity> : IRepository where TEntity : IEntity
    {

    }
}
