using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities;

public abstract class BaseBasicEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
}
