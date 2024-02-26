using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum RequestType
{
    Install = 1,
    Repair = 2,
    Remove = 3,
    Other = 4
}
