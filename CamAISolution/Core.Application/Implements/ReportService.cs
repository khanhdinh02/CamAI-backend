using System.Linq.Expressions;
using System.Text.Json;
using Core.Application.Events;
using Core.Application.Exceptions;
using Core.Application.Models;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Models.Configurations;
using Core.Domain.Models.Consumers;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class ReportService(
    IUnitOfWork unitOfWork,
    IAccountService accountService,
    IShopService shopService,
    HumanCountSubject subject,
    AiConfiguration configuration,
    IAppLogging<ReportService> logger
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

    public async Task<HumanCountDto> GetHumanCountData(DateOnly startDate, DateOnly endDate, ReportInterval interval)
    {
        var account = accountService.GetCurrentAccount();
        CheckAuthority(account);
        var shopId = account.ManagingShop!.Id;

        return await GetHumanCountDataInTimeRange(shopId, startDate, endDate, interval);
    }

    public async Task<HumanCountDto> GetHumanCountData(
        Guid shopId,
        DateOnly startDate,
        DateOnly endDate,
        ReportInterval interval
    )
    {
        // validation is already in shop service
        await shopService.GetShopById(shopId);
        return await GetHumanCountDataInTimeRange(shopId, startDate, endDate, interval);
    }

    private async Task<HumanCountDto> GetHumanCountDataInTimeRange(
        Guid shopId,
        DateOnly startDate,
        DateOnly endDate,
        ReportInterval interval
    )
    {
        List<EmployeeAndInteractionDto> columns = [];

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            // shopId -> date -> time
            var outputPath = Path.Combine(baseOutputDir, shopId.ToString("N"), date.ToFilePath());

            var lines = await ReadHumanCountLines(shopId, outputPath, date);

            // var resultForDate = lines
            //     .Select(l => JsonSerializer.Deserialize<HumanCountModel>(l)!)
            //     .OrderBy(r => r.Time)
            //     .GroupBy(r =>
            //         DateTimeHelper.CalculateTimeForInterval(
            //             r.Time,
            //             interval,
            //             startDate.ToDateTime(TimeOnly.MinValue)
            //         )
            //     )
            //     .Select(group =>
            //     {
            //         // Generate a column of the chart
            //         var count = group.Count();
            //         var orderedGroup = group.OrderBy(r => r.Total);
            //         var median = int.IsEvenInteger(count)
            //             ? (orderedGroup.ElementAt(count / 2).Total + orderedGroup.ElementAt(count / 2 - 1).Total)
            //                 / 2.0f
            //             : orderedGroup.ElementAt(count / 2).Total;
            //         return new HumanCountItemDto
            //         {
            //             Time = group.Key,
            //             Low = group.Min(r => r.Total),
            //             High = group.Max(r => r.Total),
            //             Open = group.First().Total,
            //             Close = group.Last().Total,
            //             Median = median
            //         };
            //     });

            var startDateTime = date.ToDateTime(TimeOnly.MinValue);
            var endDateTime = endDate.AddDays(1).ToDateTime(TimeOnly.MinValue);

            var timeSpan = DateTimeHelper.MapTimeSpanFromTimeInterval(interval);
            var humanCountData = lines.Select(l => JsonSerializer.Deserialize<HumanCountModel>(l)!).ToList();
            Expression<Func<Incident, bool>> criteria = i =>
                i.IncidentType == IncidentType.Interaction
                && i.ShopId == shopId
                && i.StartTime >= startDateTime
                && i.StartTime < endDateTime;
            var incidents = (await unitOfWork.Incidents.GetAsync(criteria, takeAll: true)).Values;
            var resultForDate = new List<EmployeeAndInteractionDto>();
            for (var time = startDateTime; time < date.AddDays(1).ToDateTime(TimeOnly.MinValue); time += timeSpan)
            {
                var humanCountGroup = humanCountData
                    .Where(r => r.Time >= time && r.Time < time + timeSpan)
                    .OrderBy(c => c.Time)
                    .ToList();
                HumanCountItemDto humanCountItemDto;
                if (!humanCountGroup.Any())
                {
                    humanCountItemDto = new HumanCountItemDto
                    {
                        Low = 0,
                        High = 0,
                        Open = 0,
                        Close = 0,
                        Median = 0
                    };
                }
                else
                {
                    var orderedGroup = humanCountGroup.OrderBy(r => r.Total).ToList();
                    var count = orderedGroup.Count;
                    var median = int.IsEvenInteger(count)
                        ? (orderedGroup[count / 2].Total + orderedGroup[count / 2 - 1].Total) / 2.0f
                        : orderedGroup[count / 2].Total;
                    humanCountItemDto = new HumanCountItemDto
                    {
                        Low = humanCountGroup.Min(r => r.Total),
                        High = humanCountGroup.Max(r => r.Total),
                        Open = humanCountGroup.First().Total,
                        Close = humanCountGroup.Last().Total,
                        Median = median
                    };
                }

                var interactionGroup = incidents
                    .Where(i => i.StartTime >= time && i.StartTime < time + timeSpan)
                    .ToList();
                var average = interactionGroup
                    .Where(i => i.EndTime != null)
                    .Select(i => (i.EndTime - i.StartTime)!.Value.TotalSeconds)
                    .DefaultIfEmpty(0)
                    .Average();
                var interactionDto = new InteractionCountDto
                {
                    Count = interactionGroup.Count,
                    AverageDuration = average
                };

                resultForDate.Add(
                    new EmployeeAndInteractionDto
                    {
                        Time = time,
                        HumanCount = humanCountItemDto,
                        Interaction = interactionDto
                    }
                );
            }

            columns.AddRange(resultForDate);
        }

        return new HumanCountDto
        {
            ShopId = shopId,
            StartDate = startDate,
            EndDate = endDate,
            Interval = interval,
            Data = columns,
            TotalInteraction = columns.Sum(x => x.Interaction.Count)
        };
    }

    private async Task<string[]> ReadHumanCountLines(Guid shopId, string outputPath, DateOnly date)
    {
        try
        {
            return await File.ReadAllLinesAsync(outputPath);
        }
        catch (Exception ex)
        {
            logger.Error($"Cannot find human count data for date {date} and shop {shopId}", ex);
            return [];
        }
    }

    public async Task<EdgeBoxReportDto> GetEdgeBoxReport()
    {
        var reportByStatus = unitOfWork
            .EdgeBoxes.GroupEntity(x => x.EdgeBoxStatus)
            .ToDictionary(x => x.Key, x => x.Count());
        var reportByLocation = unitOfWork
            .EdgeBoxes.GroupEntity(x => x.EdgeBoxLocation)
            .ToDictionary(x => x.Key, x => x.Count());
        return new EdgeBoxReportDto
        {
            Total = await unitOfWork.EdgeBoxes.CountAsync(),
            Location = reportByLocation,
            Status = reportByStatus
        };
    }

    public async Task<EdgeBoxInstallReportDto> GetInstallEdgeBoxReport()
    {
        var reportByStatus = unitOfWork
            .EdgeBoxInstalls.GroupEntity(x => x.EdgeBoxInstallStatus)
            .ToDictionary(x => x.Key, x => x.Count());
        var reportByActivationStatus = unitOfWork
            .EdgeBoxInstalls.GroupEntity(x => x.ActivationStatus)
            .ToDictionary(x => x.Key, x => x.Count());
        return new EdgeBoxInstallReportDto
        {
            Total = await unitOfWork.EdgeBoxInstalls.CountAsync(),
            Status = reportByStatus,
            ActivationStatus = reportByActivationStatus
        };
    }
}
