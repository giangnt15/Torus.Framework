namespace Torus.Framework.Domain.Auditting;

/// <summary>
/// The interface abstract entities with auditing Modification
/// </summary>
public interface IHasModificationInfo
{
    DateTime? UpdatedAt { get; set; }
    Guid? UpdatedBy { get; set; }
}