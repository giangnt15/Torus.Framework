using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.MultiTenancy
{
    public static class ServiceCollectionExtensions
    {
        private static TenantIdResolveOptions tenantIdResolveOptions = new();
        public static IServiceCollection AddMultiTenancy(this IServiceCollection services)
        {
            services.Configure<MultiTenancyOptions>(o =>
            {
                o.Enabled = true;
            });
            services.AddScoped(sp =>
            {
                var tenantResolver = sp.GetService<ITenantResolver>();
                return tenantResolver.Resolve();
            });
            services.AddSingleton<ITenantResolver, TenantResolver>();
            services.AddSingleton(tenantIdResolveOptions);
            return services;
        }

        public static IServiceCollection AddTenantStore<TStore>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TStore : ITenantStore
        {
            services.Add(new ServiceDescriptor(typeof(ITenantStore), typeof(TStore), lifetime));
            return services;
        }

        public static IServiceCollection AddTenantIdResolver<TResolver>(this IServiceCollection services) where TResolver : class, ITenantIdResolver
        {
            tenantIdResolveOptions.AddResolver<TResolver>();
            services.AddKeyedScoped<ITenantIdResolver, TResolver>(typeof(TResolver).AssemblyQualifiedName);
            return services;
        }

        public static IServiceCollection AddTenantIdResolver<TResolver>(this IServiceCollection services, Func<IServiceProvider, TResolver> builder) where TResolver : class, ITenantIdResolver
        {
            tenantIdResolveOptions.AddResolver<TResolver>();
            services.AddKeyedScoped<ITenantIdResolver, TResolver>(typeof(TResolver).AssemblyQualifiedName, (sp, key) =>
            {
                return builder?.Invoke(sp);
            });
            return services;
        }
    }
}
