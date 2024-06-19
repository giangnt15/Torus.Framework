using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Messaging;

namespace Torus.Framework.Saga
{
    public class SagaReplyMessage : Message, IMessage
    {
    }

    public class PseudoSagaReplyMessage : SagaReplyMessage
    {
    }
}
