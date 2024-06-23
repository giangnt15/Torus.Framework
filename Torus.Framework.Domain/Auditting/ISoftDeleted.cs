using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Domain.Auditting
{
    public interface ISoftDeleted
    {
        public bool? IsDeleted { get; set; }
    }
}
