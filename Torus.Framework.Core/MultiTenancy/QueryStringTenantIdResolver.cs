using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.MultiTenancy
{
    public class QueryStringTenantIdResolver : ITenantIdResolver
    {
        private readonly string _queryName;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QueryStringTenantIdResolver(IServiceProvider serviceProvider, string queryName)
        {
            _queryName = queryName;
            _httpContextAccessor = serviceProvider.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;

        }

        public Guid Resolve()
        {
            var hasTId = _httpContextAccessor.HttpContext.Request.Query.TryGetValue(_queryName, out var tenantIdStr);
            if (!hasTId)
            {
                return Guid.Empty;
            }
            return Guid.Parse(tenantIdStr.First());
        }
    }
}
