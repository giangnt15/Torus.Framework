using Torus.Framework.Core.Messaging;

namespace Torus.Framework.Core.Commands
{
    public class CommandEnvelop : Envelop
    {
        public string DestChannel { get; set; }
        public string ReplyChannel { get; set; }
    }
}
