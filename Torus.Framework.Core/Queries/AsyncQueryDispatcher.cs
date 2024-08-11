﻿using Microsoft.Extensions.DependencyInjection;

namespace Torus.Framework.Core.Queries
{
    public class AsyncQueryDispatcher(IServiceProvider serviceProvider) : IAsyncQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery
        {
            var handler = _serviceProvider.GetRequiredService<IAsyncQueryHandler<TQuery, TResult>>();
            return await handler.HandleAsync(query, cancellationToken);
        }
    }
}
