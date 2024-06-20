using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.MultiTenancy
{
    public class HeaderTenantIdResolver : ITenantIdResolver
    {
        private readonly string _headerName;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeaderTenantIdResolver(IServiceProvider serviceProvider, string headerName)
        {
            _headerName = headerName;
            _httpContextAccessor = serviceProvider.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;   

        }

        public Task<Guid> ResolveAsync()
        {
            var tenantIdStr = _httpContextAccessor.HttpContext.Request.Headers[_headerName];
            if (tenantIdStr.Count == 0)
            {
                return Task.FromResult(Guid.Empty);
            }
            return Task.FromResult(Guid.Parse(tenantIdStr.First()));
        }
    }
}
