using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Shared.Data;

namespace Torus.Framework.Core.Data
{
    /// <summary>
    /// Default connection string resolver, retrieve the connection string from appsettings
    /// </summary>
    public class DefaultConnectionStringResolver(IConfiguration configuration) : IConnectionStringResolver
    {
        private readonly IConfiguration _configuration = configuration;

        public Task<string> ResolveAsync(string connectionStringName = null)
        {
            if (string.IsNullOrEmpty(connectionStringName))
            {
                return Task.FromResult(_configuration.GetConnectionString(DataConstants.DefaultConnectionStringName));
            }
            return Task.FromResult(_configuration.GetConnectionString(connectionStringName));
        }
    }
}
