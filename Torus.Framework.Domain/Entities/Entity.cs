using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Domain.Entities
{
    public abstract class Entity : IEntity
    {
        public abstract string GetKeyName();
    }

    public abstract class Entity<TId> : Entity, IEntity<TId>
    {
        public TId Id { get; set; }
        public override string GetKeyName()
        {
            return nameof(Id);
        }
    }
}
