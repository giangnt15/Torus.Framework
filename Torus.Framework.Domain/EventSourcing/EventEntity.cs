using Torus.Framework.Domain.Entities;

namespace Torus.Framework.Domain.EventSourcing
{
    public class EventEntity : Entity<Guid>
    {
        public string EventType { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public Guid TriggeringMessageId { get; set; }
        public string EventData { get; set; }
        public DateTimeOffset Timestamp { get; set; }

    }
}
