using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.Data
{
    public interface IConnectionStringResolver
    {
        Task<string> ResolveAsync(string connectionStringName = null);
    }
}
