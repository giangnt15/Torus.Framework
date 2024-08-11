using Microsoft.Extensions.DependencyInjection;
using Torus.Framework.Core.Commands;
using Torus.Framework.Core.Queries;

namespace Torus.Framework.Core
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCommandAndQueryDispatcher(this IServiceCollection services)
        {
            services.AddScoped<IAsyncCommandDispatcher, AsyncCommandDispatcher>();
            services.AddScoped<IAsyncQueryDispatcher, AsyncQueryDispatcher>();
            return services;
        }
    }
}
