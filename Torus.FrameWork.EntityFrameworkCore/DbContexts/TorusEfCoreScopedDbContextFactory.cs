using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Data;
using Torus.Framework.Core.MultiTenancy;

namespace Torus.FrameWork.EntityFrameworkCore.DbContexts
{
    public class TorusEfCoreScopedDbContextFactory<TDbContext> : IDbContextFactory<TDbContext> 
        where TDbContext : TorusEfCoreDbContext<TDbContext>
    {
        private readonly IOptions<MultiTenancyOptions> _multiTenancyOptions;
        private readonly ICurrentTenant _currentTenant;
        private readonly IDbContextFactory<TDbContext> _pooledFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _connectionStringName;
        private readonly Type _resolverType;
        private TDbContext _dbContext;

        public TorusEfCoreScopedDbContextFactory(
            IServiceProvider serviceProvider, 
            string connectionStringName,
            Type resolverType)
        {
            _serviceProvider = serviceProvider;
            _currentTenant = _serviceProvider.GetRequiredService<ICurrentTenant>();
            _pooledFactory = _serviceProvider.GetRequiredService<IDbContextFactory<TDbContext>>();
            _multiTenancyOptions = _serviceProvider.GetRequiredService<IOptions<MultiTenancyOptions>>();
            _connectionStringName = connectionStringName;
            _resolverType = resolverType;
        }

        public TDbContext CreateDbContext()
        {
            if (_dbContext != null) {
                return _dbContext;
            }
            var context = _pooledFactory.CreateDbContext();
            context.SetCurrentTenant(_currentTenant);
            var resolver = _serviceProvider.GetRequiredService(_resolverType) as IConnectionStringResolver;
            var connectionString = resolver!.ResolveAsync(_connectionStringName).Result;
            context.Database.SetConnectionString(connectionString);
            _dbContext = context;
            return context;
        }

        public async Task<TDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
        {
            if (_dbContext != null)
            {
                return _dbContext;
            }
            var context = await _pooledFactory.CreateDbContextAsync(cancellationToken);
            context.SetCurrentTenant(_currentTenant);
            var resolver = _serviceProvider.GetRequiredService(_resolverType) as IConnectionStringResolver;
            var connectionString = await resolver!.ResolveAsync(_connectionStringName);
            context.Database.SetConnectionString(connectionString);
            _dbContext = context;
            return context;
        }

    }
}
