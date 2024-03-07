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

    public async Task<List<HumanCountModel>> GetHumanCountDataForDate(DateOnly date)
    {
        var account = accountService.GetCurrentAccount();
        CheckAuthority(account);
        var shopId = account.ManagingShop!.Id;

        return await GetHumanCountDataForShop(date, shopId);
    }

    public async Task<List<HumanCountModel>> GetHumanCountDataForDate(Guid shopId, DateOnly date)
    {
        // validation is already in shop service
        await shopService.GetShopById(shopId);
        return await GetHumanCountDataForShop(date, shopId);
    }

    private async Task<List<HumanCountModel>> GetHumanCountDataForShop(DateOnly date, Guid shopId)
    {
        // shopId -> date -> time
        var outputPath = Path.Combine(baseOutputDir, shopId.ToString("N"), date.ToFilePath());
        try
        {
            var lines = await File.ReadAllLinesAsync(outputPath);
            return lines.Select(l => JsonSerializer.Deserialize<HumanCountModel>(l)!).ToList();
        }
        catch (Exception)
        {
            throw new NotFoundException($"Cannot find human count data for date {date} and shop {shopId}");
        }
    }
}
