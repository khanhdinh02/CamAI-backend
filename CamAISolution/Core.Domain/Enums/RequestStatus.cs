using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum RequestStatus
{
    Open = 1,
    Canceled,
    Done,
    Rejected
}
