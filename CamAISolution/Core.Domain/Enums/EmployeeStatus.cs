using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum EmployeeStatus
{
    Active = 1,
    Inactive = 2
}
