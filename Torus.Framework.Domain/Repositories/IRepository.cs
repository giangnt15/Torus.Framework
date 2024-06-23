using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Domain.Entities;

namespace Torus.Framework.Domain.Repositories
{
    public interface IRepository
    {

    }

    public interface IRepository<TKey, TEntity> : IRepository where TEntity : IEntity<TKey>
    {

    }

    public interface IRepository<TEntity> : IRepository where TEntity : IEntity
    {

    }
}
