using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Messaging;
using Torus.Framework.Domain.Entities;

namespace Torus.Framework.Domain.Events
{
    public interface IEvent : IMessage, IVersioned
    {
        /// <summary>
        /// Id of the object/aggregate generating this event
        /// </summary>
        public object SourceId { get; set; }
        /// <summary>
        /// Id of the message when proccessed will yeild this event
        /// </summary>
        public Guid TriggeringMessageId { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
