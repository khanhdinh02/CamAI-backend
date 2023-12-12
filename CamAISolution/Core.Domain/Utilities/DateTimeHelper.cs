namespace Core.Domain.Utilities;
public static class DateTimeHelper
{
    public static DateTime VNDateTime => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
}