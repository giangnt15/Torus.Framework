using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.MultiTenancy;
using Torus.Framework.Domain.Auditting;

namespace Torus.FrameWork.EntityFrameworkCore.DbContexts
{
    public class TorusEfCoreDbContext<TDbContext> : DbContext
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
            Database.Migrate();
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
