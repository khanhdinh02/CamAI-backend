using System.Text.Json;
using Core.Application.Events;
using Core.Application.Exceptions;
using Core.Application.Models;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Models.Configurations;
using Core.Domain.Models.Consumers;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class ReportService(
    IAccountService accountService,
    IShopService shopService,
    HumanCountSubject subject,
    AiConfiguration configuration
) : IReportService
{
    private readonly string baseOutputDir = configuration.OutputDirectory;

    public Task<ICircularBuffer<HumanCountModel>> GetHumanCountStream()
    {
        var account = accountService.GetCurrentAccount();
        CheckAuthority(account);

        var buffer = CreateHumanCountBufferResult(account.ManagingShop!.Id);
        return Task.FromResult<ICircularBuffer<HumanCountModel>>(buffer);
    }

    public async Task<ICircularBuffer<HumanCountModel>> GetHumanCountStream(Guid shopId)
    {
        // validation is already in shop service
        await shopService.GetShopById(shopId);
        return CreateHumanCountBufferResult(shopId);
    }

    private static void CheckAuthority(Account account)
    {
        if (account.Role != Role.ShopManager)
            throw new BadRequestException("Please specify a shop id");
        if (account.ManagingShop == null)
            throw new BadRequestException("Account is not manging any shop");
    }

    private HumanCountBuffer CreateHumanCountBufferResult(Guid shopId) => new(subject, shopId);

    public async Task<IEnumerable<HumanCountDto>> GetHumanCountData(DateOnly date, ReportTimeRange timeRange)
    {
        var account = accountService.GetCurrentAccount();
        CheckAuthority(account);
        var shopId = account.ManagingShop!.Id;

        return await GetHumanCountDataInTimeRange(shopId, date, ReportTimeRange.Day);
    }

    public async Task<IEnumerable<HumanCountDto>> GetHumanCountData(
        Guid shopId,
        DateOnly date,
        ReportTimeRange timeRange
    )
    {
        // validation is already in shop service
        await shopService.GetShopById(shopId);
        return await GetHumanCountDataInTimeRange(shopId, date, ReportTimeRange.Day);
    }

    private async Task<IEnumerable<HumanCountDto>> GetHumanCountDataInTimeRange(
        Guid shopId,
        DateOnly startDate,
        ReportTimeRange timeRange
    )
    {
        IEnumerable<HumanCountDto> result = [];

        // Calculate EndDate and GroupByTime base on time range.
        // GroupByTime is a Func to pass to GroupBy method.
        // Each group is a column in the chart.
        var (endDate, groupByTime) = timeRange switch
        {
            ReportTimeRange.Day
                => (
                    startDate.AddDays(1),
                    (Func<HumanCountModel, DateTime>)(
                        m =>
                            new DateTime(
                                DateOnly.FromDateTime(m.Time),
                                new TimeOnly(m.Time.Hour, m.Time.Minute / 30 * 30), // 30 min gap
                                DateTimeKind.Utc
                            )
                    )
                ),
            ReportTimeRange.Week
                => (
                    startDate.AddDays(7),
                    m =>
                        new DateTime(
                            DateOnly.FromDateTime(m.Time),
                            new TimeOnly(m.Time.Hour / 12 * 12), // 1/2 day gap
                            DateTimeKind.Utc
                        )
                ),
            ReportTimeRange.Month
                => (
                    startDate.AddMonths(1),
                    m =>
                        new DateTime(
                            DateOnly.FromDateTime(m.Time), // 1 day gap
                            TimeOnly.MinValue,
                            DateTimeKind.Utc
                        )
                ),
            _ => throw new ArgumentOutOfRangeException(nameof(timeRange), timeRange, null)
        };

        for (var date = startDate; date < endDate; date = date.AddDays(1))
        {
            // shopId -> date -> time
            var outputPath = Path.Combine(baseOutputDir, shopId.ToString("N"), date.ToFilePath());
            try
            {
                var lines = await File.ReadAllLinesAsync(outputPath);

                var resultForDate = lines
                    .Select(l => JsonSerializer.Deserialize<HumanCountModel>(l)!)
                    .OrderBy(r => r.Time)
                    .GroupBy(groupByTime)
                    .Select(group =>
                    {
                        // Generate a column of the chart
                        var count = group.Count();
                        var orderedGroup = group.OrderBy(r => r.Total);
                        var median = int.IsEvenInteger(count)
                            ? (orderedGroup.ElementAt(count / 2).Total + orderedGroup.ElementAt(count / 2 - 1).Total)
                                / 2.0f
                            : orderedGroup.ElementAt(count / 2).Total;
                        return new HumanCountDto
                        {
                            ShopId = shopId,
                            Time = group.Key,
                            Low = group.Min(r => r.Total),
                            High = group.Max(r => r.Total),
                            Open = group.First().Total,
                            Close = group.Last().Total,
                            Median = median
                        };
                    });
                result = result.Concat(resultForDate);
            }
            catch (Exception)
            {
                throw new NotFoundException($"Cannot find human count data for date {date} and shop {shopId}");
            }
        }
        return result;
    }
}
