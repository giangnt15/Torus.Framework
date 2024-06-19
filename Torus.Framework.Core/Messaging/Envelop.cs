using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.Messaging
{
    public class Envelop
    {
        public string Message { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public static Envelop Create(string message, Dictionary<string, string> headers)
        {
            return new Envelop
            {
                Message = message,
                Headers = headers
            };
        }

        public static Envelop Create(IMessage message, Dictionary<string, string> headers)
        {
            return new Envelop
            {
                Message = message.ToJson(),
                Headers = headers
            };
        }

        public string GetHeader(string key)
        {
            return Headers[key];
        }
    }
}
