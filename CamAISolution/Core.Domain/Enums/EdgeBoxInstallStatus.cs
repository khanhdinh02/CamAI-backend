using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum EdgeBoxInstallStatus
{
    Working = 1,
    Unhealthy = 2,
    Disabled = 3,
    New = 4,

    [Obsolete]
    Connected = 5
}
