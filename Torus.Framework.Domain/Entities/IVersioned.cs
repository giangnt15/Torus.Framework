namespace Torus.Framework.Domain.Entities
{
    /// <summary>
    /// Interface for versioned entity.
    /// Used for implementing optimistic locking
    /// </summary>
    public interface IVersioned
    {
        public int Version { get; set; }
    }
}
