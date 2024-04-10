using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum ReportTimeRange
{
    Day,
    Week,
    Month
}
