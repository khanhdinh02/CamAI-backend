using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum RequestStatus
{
    Open = 1,
    Canceled = 2,
    Done = 3,
    Rejected = 4
}
