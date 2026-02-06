namespace FitJournal.Core.Entities;

/// <summary>
/// Base class for all entities with common audit properties
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
