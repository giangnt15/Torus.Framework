using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Data;
using Torus.FrameWork.EntityFrameworkCore.DbContexts;

namespace Torus.FrameWork.EntityFrameworkCore
{
    public static class ServiceCollectionExtensions
    {
        public static TorusDbContextFactoryBuilder<TDbContext, TResolver> AddTorusDbContext<TDbContext, TResolver>(this IServiceCollection services)
            where TDbContext : TorusEfCoreDbContext<TDbContext>
            where TResolver : IConnectionStringResolver
        {
            return new TorusDbContextFactoryBuilder<TDbContext, TResolver>(services);
        }
    }
}
