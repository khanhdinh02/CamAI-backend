using System.ComponentModel.DataAnnotations;

namespace Core.Domain;

public abstract class BaseBasicEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
}
