using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Data;

namespace Torus.FrameWork.EntityFrameworkCore.DbContexts
{
    public class TorusDbContextFactoryBuilder<TDbContext, TResolver>
        where TDbContext : TorusEfCoreDbContext<TDbContext>
        where TResolver : IConnectionStringResolver
    {
        private readonly IServiceCollection _services;
        private bool _setupDone;

        public TorusDbContextFactoryBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public void UseMysql(string connectionStringName, ServerVersion serverVersion, Action<MySqlDbContextOptionsBuilder>? mySqlOptionsAction = null)
        {
            if (_setupDone) throw new InvalidOperationException("The setup is done, can't setup again!");
            _services.AddScoped(sp =>
            {
                return new TorusEfCoreScopedDbContextFactory<TDbContext>(sp, connectionStringName, typeof(TResolver));
            });

            _services.AddPooledDbContextFactory<TDbContext>((sp, oa) =>
            {
                oa.UseMySql(serverVersion, mySqlOptionsAction);
            });

            _services.AddScoped(
                sp => sp.GetRequiredService<TorusEfCoreScopedDbContextFactory<TDbContext>>().CreateDbContext());
            _setupDone = true;
        }

        public void UseNpgsql(string connectionStringName, Action<NpgsqlDbContextOptionsBuilder>? npgSqlOptionsAction = null)
        {
            if (_setupDone) throw new InvalidOperationException("The setup is done, can't setup again!");
            _services.AddScoped(sp =>
            {
                return new TorusEfCoreScopedDbContextFactory<TDbContext>(sp, connectionStringName, typeof(TResolver));
            });

            _services.AddPooledDbContextFactory<TDbContext>((sp, oa) =>
            {
                oa.UseNpgsql(npgSqlOptionsAction);
            });

            _services.AddScoped(
                sp => sp.GetRequiredService<TorusEfCoreScopedDbContextFactory<TDbContext>>().CreateDbContext());
            _setupDone = true;

        }
    }
}
