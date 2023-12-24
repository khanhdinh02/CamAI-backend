using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities.Base;

public abstract class UpdatableEntity<T> : BaseEntity<T>
{
    [Timestamp]
    public byte[]? Timestamp { get; set; }
}
