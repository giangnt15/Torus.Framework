using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.MultiTenancy
{
    public class TenantIdResolveOptions
    {
        internal List<string> ResolverTypes { get; }

        internal TenantIdResolveOptions()
        {
            ResolverTypes = [];
        }


        internal TenantIdResolveOptions AddResolver<TResolver>() where TResolver : class, ITenantIdResolver
        {
            if (!ResolverTypes.Contains(typeof(TResolver).AssemblyQualifiedName))
            {
                ResolverTypes.Add(typeof(TResolver).AssemblyQualifiedName);
            }
            return this;
        }
    }
}
