using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Domain.Shared.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(int code, string message) : base(message)
        {
            HResult = code;
        }
    }
}
