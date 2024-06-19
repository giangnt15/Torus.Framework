using Torus.Framework.Domain.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Saga.Exceptions
{
    internal class SagaUnprocessableException : BusinessException
    {
        public SagaUnprocessableException(string message) : base(10000, message)
        {
        }
    }
}
