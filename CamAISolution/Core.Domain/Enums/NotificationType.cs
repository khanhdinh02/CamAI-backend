using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum NotificationType
{
    EdgeBoxUnhealthy = 0,
    EdgeBoxInstallActivation = 1,
    EdgeBoxHealthy = 2
}
