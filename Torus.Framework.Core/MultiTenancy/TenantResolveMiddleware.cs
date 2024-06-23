using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Shared.Extensions;

namespace Torus.Framework.Core.MultiTenancy
{
    public class TenantResolveMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        public async Task InvokeAsync(HttpContext context)
        {
            //if (context.Request.IsHealthz())
            //{
            //    await _next(context);
            //    return;
            //}

            //if (context.RequestServices.GetService(typeof(ICurrentTenant)) is ICurrentTenant currentTenant)
            //{
            //    if (context.RequestServices.GetService(typeof(ITenantResolver)) is ITenantResolver tenantResolver)
            //    {
            //        var tenant = await tenantResolver.ResolveAsync();
            //        if (tenant != null)
            //        {
            //            currentTenant.ChangeTenant(tenant);
            //        }
            //    }
            //}
            await _next(context);
        }
    }
}
