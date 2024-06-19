using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.Messaging
{
    public interface IMessageConsumer
    {
        void Subscribe<TEnvelop>(string subcriberId, string channel, Action<TEnvelop> handler) where TEnvelop : Envelop;
        void Subscribe<TEnvelop>(string subcriberId, string channel, Func<TEnvelop, Task> handler) where TEnvelop : Envelop;
        void Stop();
    }
}
