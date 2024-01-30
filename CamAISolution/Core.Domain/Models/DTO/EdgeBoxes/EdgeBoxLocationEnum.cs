using Core.Domain.Models.Attributes;

namespace Core.Domain.DTO;

[Lookup]
public static class EdgeBoxLocationEnum
{
    public const int Idle = 1;
    public const int Installing = 2;
    public const int Occupied = 3;
    public const int Uninstalling = 4;
    public const int Disposed = 5;
}
