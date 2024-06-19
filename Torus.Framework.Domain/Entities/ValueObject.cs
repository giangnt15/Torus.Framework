using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Domain.Entities
{
    /// <summary>
    /// Base class for all value objects.
    /// Using record so as not to have to override Equals.
    /// All Property must be string or primitive types
    /// </summary>
    public record class ValueObject : IValueObject
    {
    }
}
