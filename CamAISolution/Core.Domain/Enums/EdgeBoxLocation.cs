using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum EdgeBoxLocation
{
    Idle = 1,
    Installing = 2,
    Occupied = 3,
    Uninstalling = 4
}
