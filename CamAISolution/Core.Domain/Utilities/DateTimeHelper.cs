using Core.Domain.Enums;

namespace Core.Domain.Utilities;

public static class DateTimeHelper
{
    public static DateTime VNDateTime =>
        TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));

    /// <summary>Calculate the key (time) used for grouping data (e.g. <c>GroupBy</c> method)</summary>
    /// <remarks>
    ///     Valid <c>interval</c>: <see cref="ReportInterval.HalfHour"/>, <see cref="ReportInterval.Hour"/>,
    ///     <see cref="ReportInterval.HalfDay"/>, <see cref="ReportInterval.Day"/>
    /// </remarks>
    /// <seealso cref="CalculateTimeForInterval(DateTime,ReportInterval,DateTime)"/>
    /// <param name="dateTime"></param>
    /// <param name="interval"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">If <c>interval</c> is not the ones listed above</exception>
    public static DateTime CalculateTimeForInterval(DateTime dateTime, ReportInterval interval)
    {
        return interval switch
        {
            ReportInterval.HalfHour
                => new DateTime(
                    DateOnly.FromDateTime(dateTime),
                    new TimeOnly(dateTime.Hour, dateTime.Minute / 30 * 30)
                ),
            ReportInterval.Hour => new DateTime(DateOnly.FromDateTime(dateTime), new TimeOnly(dateTime.Hour)),
            ReportInterval.HalfDay
                => new DateTime(DateOnly.FromDateTime(dateTime), new TimeOnly(dateTime.Hour / 12 * 12)),
            ReportInterval.Day => dateTime.Date,
            _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, null)
        };
    }

    /// <summary>Calculate the key (time) used for grouping data (e.g. <c>GroupBy</c> method)</summary>
    /// <remarks>
    ///     Valid <c>interval</c>: <see cref="ReportInterval.Week"/>
    /// </remarks>
    /// <seealso cref="CalculateTimeForInterval(DateTime,ReportInterval)"/>
    /// <param name="dateTime"></param>
    /// <param name="interval"></param>
    /// <param name="startDateTime"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static DateTime CalculateTimeForInterval(DateTime dateTime, ReportInterval interval, DateTime startDateTime)
    {
        return interval switch
        {
            ReportInterval.Week => dateTime.Date.AddDays((startDateTime.Date - dateTime.Date).Days / 7 * 7),
            ReportInterval.HalfHour
            or ReportInterval.Hour
            or ReportInterval.HalfDay
            or ReportInterval.Day
                => CalculateTimeForInterval(dateTime, interval),
            _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, null)
        };
    }
}
