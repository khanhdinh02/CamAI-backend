using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum AccountStatus
{
    New = 1,
    Active = 2,
    Inactive = 3
}
