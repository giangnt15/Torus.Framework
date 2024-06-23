using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.MultiTenancy
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseMultiTenancy(this WebApplication app)
        {
            //app.UseMiddleware<TenantResolveMiddleware>();
            return app;
        }
    }
}
