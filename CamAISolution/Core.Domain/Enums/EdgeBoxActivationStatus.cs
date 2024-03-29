using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum EdgeBoxActivationStatus
{
    NotActivated = 0,
    Activated = 1,
    Pending = 2,
    Failed = 3
}
