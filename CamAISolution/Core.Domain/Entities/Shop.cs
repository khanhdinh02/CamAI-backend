using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Shop : BaseEntity
{
    public string Name { get; set; } = null!;
}
