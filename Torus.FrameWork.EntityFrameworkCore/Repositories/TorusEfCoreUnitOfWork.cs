using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Domain.Repositories;
using Torus.FrameWork.EntityFrameworkCore.DbContexts;

namespace Torus.FrameWork.EntityFrameworkCore.Repositories
{
    public abstract class TorusEfCoreUnitOfWork<TDbContext> : IUnitOfWork, IAsyncDisposable
        where TDbContext : TorusEfCoreDbContext<TDbContext>
    {
        protected readonly TorusEfCoreScopedDbContextFactory<TDbContext> ContextFactory;
        private TDbContext _dbContext;

        protected IDbContextTransaction Transaction { get; private set; }

        public TorusEfCoreUnitOfWork(TorusEfCoreScopedDbContextFactory<TDbContext> contextFactory)
        {
            ContextFactory = contextFactory;
        }

        private async Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
        {
            _dbContext ??= await ContextFactory.CreateDbContextAsync(cancellationToken);
            return _dbContext;
        }

        public async Task BeginTransactionAsync()
        {
            await GetDbContextAsync();
            Transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (Transaction != null)
            {
                await Transaction.CommitAsync();
            }
            else
            {
                await _dbContext.SaveChangesAsync();
            }

        }

        public virtual TRepo GetRepository<TRepo>() where TRepo : IRepository
        {
            throw new NotImplementedException();
        }

        public async Task RollbackAsync()
        {
            await Transaction.RollbackAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (Transaction != null)
            {
                await Transaction.DisposeAsync();
            }
            GC.SuppressFinalize(this);
        }
    }
}
