﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.MultiTenancy
{
    public interface ITenantIdResolver
    {
        Guid Resolve();
    }
}
