using Core.Domain.Models.Attributes;

namespace Core.Domain.DTO;

[Lookup]
public static class EdgeBoxInstallStatusEnum
{
    public const int Valid = 1;
    public const int Expired = 2;
}
