using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Commands;

namespace Torus.Framework.Core.Messaging
{
    public class EnvelopFactory
    {
        public static Envelop Create<T>(T message, Dictionary<string, string> headers) where T : IMessage
        {
            return new Envelop
            {
                Message = message.ToJson(),
                Headers = headers
            };
        }

        public static CommandEnvelop CreateCommandEnvelop<T>(T message,
            Dictionary<string, string> headers, string channel, string replyChannel) where T : ICommand
        {
            return new CommandEnvelop
            {
                Message = message.ToJson(),
                Headers = headers,
                DestChannel = channel,
                ReplyChannel = replyChannel
            };
        }

    }
}
