using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.Shared.Sagas
{
    public static class SagaReplyHeaders
    {
        public const string SAGA_TYPE = "SagaType";
        public const string SAGA_ID = "SagaId";
        public const string OUTCOME = "Outcome";
    }
}
