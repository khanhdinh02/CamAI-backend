using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum EdgeBoxInstallStatus
{
    Valid = 1,
    Expired = 2
}
