using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.MultiTenancy
{
    public class TenantConfig
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Dictionary<string, string> ConnectionStrings { get; }
    }
}
