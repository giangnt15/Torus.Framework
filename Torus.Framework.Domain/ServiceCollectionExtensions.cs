using Microsoft.Extensions.DependencyInjection;

using Torus.Framework.Domain.Events;

namespace Torus.Framework.Domain
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEventDispatchers(this IServiceCollection services)
        {
            services.AddScoped<IAsyncEventDispatcher, AsyncEventDispatcher>();
            return services;
        }
    }
}
