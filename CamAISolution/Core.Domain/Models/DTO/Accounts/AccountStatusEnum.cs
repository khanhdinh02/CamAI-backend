using Core.Domain.Models.Attributes;

namespace Core.Domain.DTO;

[Lookup]
public static class AccountStatusEnum
{
    public const int New = 1;
    public const int Active = 2;
    public const int Inactive = 3;
}
