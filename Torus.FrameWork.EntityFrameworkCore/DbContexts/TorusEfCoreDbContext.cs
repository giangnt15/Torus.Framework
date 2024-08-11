using Microsoft.EntityFrameworkCore;
using Torus.Framework.Core.MultiTenancy;
using Torus.Framework.Domain.Auditting;
using Torus.Framework.Domain.Repositories;

namespace Torus.FrameWork.EntityFrameworkCore.DbContexts
{
    public class TorusEfCoreDbContext<TDbContext> : DbContext, ITorusDbContext
        where TDbContext : TorusEfCoreDbContext<TDbContext>
    {
        protected ICurrentTenant? CurrentTenant { get; private set; }

        public TorusEfCoreDbContext(DbContextOptions<TDbContext> options) : base(options)
        {

        }

        internal void SetCurrentTenant(ICurrentTenant currentTenant)
        {
            CurrentTenant = currentTenant;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;
                if (ShouldFilterTenant(clrType))
                {
                    modelBuilder.Entity(clrType).AddQueryFilter<IMultiTenant>(x => x.TenantId == CurrentTenant!.Id);
                }
                if (ShouldFilterSoftDelete(clrType))
                {
                    modelBuilder.Entity(clrType).AddQueryFilter<ISoftDeleted>(x => x.IsDeleted == null || x.IsDeleted == false);
                }
            }
        }

        protected virtual bool ShouldFilterEntity(Type entityType)
        {
            if (typeof(IMultiTenant).IsAssignableFrom(entityType))
            {
                return true;
            }

            if (typeof(ISoftDeleted).IsAssignableFrom(entityType))
            {
                return true;
            }

            return false;
        }

        protected virtual bool ShouldFilterTenant(Type entityType)
        {
            if (typeof(IMultiTenant).IsAssignableFrom(entityType))
            {
                return true;
            }

            return false;
        }

        protected virtual bool ShouldFilterSoftDelete(Type entityType)
        {
            if (typeof(ISoftDeleted).IsAssignableFrom(entityType))
            {
                return true;
            }

            return false;
        }
    }
}
