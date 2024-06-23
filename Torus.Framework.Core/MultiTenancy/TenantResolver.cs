using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.MultiTenancy
{
    public class TenantResolver : ITenantResolver
    {
        public readonly IServiceProvider _serviceProvider;

        public TenantResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICurrentTenant Resolve()
        {
            using var scope = _serviceProvider.CreateScope();
            var resolveOptions = scope.ServiceProvider.GetService(typeof(TenantIdResolveOptions)) as TenantIdResolveOptions;
            foreach (var typeName in resolveOptions.ResolverTypes)
            {
                var resolver = scope.ServiceProvider.GetRequiredKeyedService<ITenantIdResolver>(typeName);
                var tenantId = resolver.Resolve();
                if (tenantId != Guid.Empty)
                {
                    return new CurrentTenant() { Id = tenantId };
                }
            }
            return null;
        }
    }
}
