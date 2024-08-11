using Torus.Framework.Domain.Entities;
using Torus.Framework.Domain.Repositories;
using Torus.FrameWork.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Torus.FrameWork.EntityFrameworkCore.Repositories
{
    public class BaseEfCoreRepository<TKey, TEntity, TDbContext> : IRepository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
        where TDbContext : TorusEfCoreDbContext<TDbContext>
    {
        protected readonly TDbContext DbContext;

        public BaseEfCoreRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbContext.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<TEntity>().AsNoTracking().AnyAsync(x => x.Id.Equals(key), cancellationToken);
        }

        public async Task<TEntity> LoadAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var entity = await DbContext.Set<TEntity>().FirstOrDefaultAsync(cancellationToken);
            return entity;
        }

        public Task SaveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entry = DbContext.Entry(entity);
            if (entry.State == EntityState.Detached) { 
                DbContext.Add(entry);
            }else if (entry.State == EntityState.Unchanged)
            {
                entry.State = EntityState.Modified;
            }
            return Task.CompletedTask;
        }
    }
}
