using Microsoft.EntityFrameworkCore;
using Torus.Framework.Core.Serialization;
using Torus.Framework.Domain.Events;
using Torus.Framework.Domain.EventSourcing;
using Torus.Framework.Domain.Repositories;
using Torus.FrameWork.EntityFrameworkCore.DbContexts;

namespace Torus.FrameWork.EntityFrameworkCore.Repositories
{
    public class BaseEfCoreEventSourcedRepository<TKey, TAggregateRoot, TDbContext, TDbContextFactory>
        : IEventSourcedRepository<TKey, TAggregateRoot>
        where TAggregateRoot : EventSourcedAggregateRoot<TKey>, new()
        where TDbContext : TorusEfCoreDbContext<TDbContext>
        where TDbContextFactory : TorusEfCoreScopedDbContextFactory<TDbContext>
    {

        protected readonly TorusEfCoreScopedDbContextFactory<TDbContext> DbContextFactory;
        public BaseEfCoreEventSourcedRepository(TorusEfCoreScopedDbContextFactory<TDbContext> factory)
        {
            DbContextFactory = factory;
        }

        protected TDbContext DbContext { get; private set; }

        protected async Task<TDbContext> GetDbContextAsync()
        {
            DbContext ??= await DbContextFactory.CreateDbContextAsync();
            return DbContext;
        }

        public Task DeleteAsync(TAggregateRoot entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var entityId = GetEntityId(key);
            return await DbContext.Set<EventEntity>().AnyAsync(x => x.EntityId == entityId, cancellationToken);
        }

        public async Task<TAggregateRoot> LoadAsync(TKey key, CancellationToken cancellationToken = default)
        {
            if (!await ExistAsync(key, cancellationToken)) return null;
            var entityId = GetEntityId(key);
            var eventEntities = await DbContext.Set<EventEntity>().AsNoTracking().Where(x => x.EntityId == entityId).ToListAsync(cancellationToken);
            var aggregateRoot = new TAggregateRoot();
            foreach (var entity in eventEntities)
            {
                var evt = JsonSerializerFactory.GetSerializer<NewtonSoftJsonSerializer>().Deserialize(entity.EventData, Type.GetType(entity.EventType)) as IEvent;
                aggregateRoot.Load(evt);
            }
            return aggregateRoot;
        }

        public async Task SaveAsync(TAggregateRoot entity, CancellationToken cancellationToken = default)
        {
            var events = entity.GetDistributedEvents();
            List<EventEntity> eventEntities = [];
            foreach (var evt in events)
            {
                eventEntities.Add(new EventEntity()
                {
                    Id = evt.Id,
                    EntityId = GetEntityId(entity.Id),
                    EntityType = typeof(TAggregateRoot).Name,
                    EventData = JsonSerializerFactory.GetSerializer<NewtonSoftJsonSerializer>().Serialize(evt),
                    EventType = evt.GetType().AssemblyQualifiedName,
                    Timestamp = evt.Timestamp,
                    TriggeringMessageId = evt.TriggeringMessageId
                });
            }
            await DbContext.Set<EventEntity>().AddRangeAsync(eventEntities, cancellationToken);
            entity.ClearDistributedEvents();
            entity.ClearLocalEvents();
        }

        private static string GetEntityId(TKey key)
        {
            return $"{typeof(TAggregateRoot).Name}_{key}";
        }
        private static string GetEntityId(TAggregateRoot entity)
        {
            return $"{typeof(TAggregateRoot).Name}_{entity.Id}";
        }
    }
}
