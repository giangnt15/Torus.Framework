using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Data;
using Torus.Framework.Core.Shared.Data;

namespace Torus.Framework.Core.MultiTenancy
{
    public class MultiTenantConnectionStringResolver : IConnectionStringResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrentTenant _currentTenant;
        public MultiTenantConnectionStringResolver(IServiceProvider serviceProvider) { 
            _serviceProvider = serviceProvider;
            _currentTenant = _serviceProvider.GetService<CurrentTenant>();
        }

        public async Task<string> ResolveAsync(string connectionStringName = DataConstants.DefaultConnectionStringName)
        {
            var tenantStore = _serviceProvider.GetRequiredService<ITenantStore>();
            var tConfig = await tenantStore.GetAsync(tenantId: _currentTenant.Id);
            return tConfig.ConnectionStrings[connectionStringName];
        }
    }
}
