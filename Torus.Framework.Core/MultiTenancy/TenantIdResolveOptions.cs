using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.MultiTenancy
{
    public class TenantIdResolveOptions
    {
        public List<ITenantIdResolver> Resolvers { get; }

        public TenantIdResolveOptions()
        {
            Resolvers = [];
        }
    }
}
