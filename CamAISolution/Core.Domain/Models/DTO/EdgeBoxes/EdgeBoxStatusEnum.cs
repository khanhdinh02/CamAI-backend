using Core.Domain.Models.Attributes;

namespace Core.Domain.DTO;

[Lookup]
public static class EdgeBoxStatusEnum
{
    public const int Active = 1;
    public const int Inactive = 2;
    public const int Broken = 3;
}
