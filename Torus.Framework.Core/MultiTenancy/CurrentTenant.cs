using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.MultiTenancy
{
    public class CurrentTenant : ICurrentTenant
    {
        public Guid Id { get; internal set; }

        public string Name { get; internal set; }

        public CurrentTenant()
        {

        }

        public void ChangeTenant(ICurrentTenant tenant)
        {
            Id = tenant.Id;
            Name = tenant.Name;
        }

        public static CurrentTenant Create(Guid Id, string name)
        {
            return new CurrentTenant() { Id = Id, Name = name };
        }
    }
}
