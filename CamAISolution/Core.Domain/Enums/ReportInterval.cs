using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum ReportInterval
{
    HalfHour = 0,
    Hour = 1,
    HalfDay = 2,
    Day = 3,
    Week = 4
}
