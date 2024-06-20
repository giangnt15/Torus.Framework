using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.MultiTenancy
{
    public class CurrentTenant : ICurrentTenant
    {
        public Guid Id { get; }

        public string Name { get; }
    }
}
