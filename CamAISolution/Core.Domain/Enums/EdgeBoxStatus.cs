using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum EdgeBoxStatus
{
    Active = 1,
    Inactive,
    Broken
}
