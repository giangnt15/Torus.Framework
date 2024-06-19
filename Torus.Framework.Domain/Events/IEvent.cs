using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Messaging;

namespace Torus.Framework.Domain.Events
{
    public interface IEvent : IMessage
    {
        /// <summary>
        /// Id of the object/aggregate generating this event
        /// </summary>
        public object SourceId { get; set; }
        /// <summary>
        /// Id of the message when proccessed will yeild this event
        /// </summary>
        public Guid TriggeringMessageId { get; set; }

        /// <summary>
        /// Version of the object/aggregate generating this event
        /// </summary>
        public int Version { get; set; }
    }
}
