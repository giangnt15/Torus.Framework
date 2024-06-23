namespace Torus.Framework.Domain.Auditting;

/// <summary>
/// The interface abstract entities with auditing Creation
/// </summary>
public interface IHasCreationInfo
{
    DateTime CreatedAt { get; set; }
    Guid CreatedBy { get; set; }
}