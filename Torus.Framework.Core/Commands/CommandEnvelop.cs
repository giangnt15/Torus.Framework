using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Messaging;

namespace Torus.Framework.Core.Commands
{
    public class CommandEnvelop : Envelop
    {
        public string DestChannel { get; set; }
        public string ReplyChannel { get; set; }
    }
}
