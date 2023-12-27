using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities.Base;

public abstract class BaseEntity<T>
{
    [Key]
    public virtual T Id { get; set; } = default!;
}
