using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Domain.Entities;
using Torus.Framework.Domain.Repositories;
using Torus.FrameWork.EntityFrameworkCore.DbContexts;

namespace Torus.FrameWork.EntityFrameworkCore.Repositories
{
    public class BaseEfCoreRepository<TKey, TEntity, TDbContext> : IRepository<TKey, TEntity> 
        where TEntity : IEntity<TKey>
        where TDbContext : TorusEfCoreDbContext<TDbContext>
    {
        protected readonly TDbContext DbContext;
        private readonly bool _inUow = false;

        public BaseEfCoreRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
