using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum EdgeBoxInstallStatus
{
    Healthy = 1,
    Unhealthy = 2,
    Disabled = 3,
}
