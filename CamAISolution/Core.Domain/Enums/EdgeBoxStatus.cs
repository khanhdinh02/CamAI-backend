using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum EdgeBoxStatus
{
    Active = 1,
    Inactive = 2,
    Broken = 3,
    Disposed = 4
}
