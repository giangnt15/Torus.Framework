using Torus.Framework.Core.Messaging;

namespace Torus.Framework.Domain.Events
{
    public abstract class BaseEvent : Message, IEvent
    {
        public object SourceId { get; set; }
        public Guid TriggeringMessageId { get; set; }
        public int Version { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    }
}
