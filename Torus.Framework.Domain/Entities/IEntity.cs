using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Domain.Entities
{
    public interface IEntity
    {

        public string GetKeyName();
    }

    public interface IEntity<TId> : IEntity
    {
        TId Id { get; set; }
    }

}
