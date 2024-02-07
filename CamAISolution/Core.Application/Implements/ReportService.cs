using System.Text.Json;
using Core.Application.Events;
using Core.Application.Exceptions;
using Core.Application.Models;
using Core.Domain.DTO;
using Core.Domain.Entities;
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
    ClassifierSubject subject,
    AiConfiguration configuration
) : IReportService
{
    private readonly string baseOutputDir = configuration.OutputDirectory;

    public Task<ICircularBuffer<ClassifierModel>> GetClassifierStream()
    {
        var account = accountService.GetCurrentAccount();
        CheckAuthority(account);

        var buffer = CreateClassifierBufferResult(account.ManagingShop!.Id);
        return Task.FromResult<ICircularBuffer<ClassifierModel>>(buffer);
    }

    private static void CheckAuthority(Account account)
    {
        if (!account.HasRole(RoleEnum.ShopManager))
            throw new BadRequestException("Please specify a shop id");
        if (account.ManagingShop == null)
            throw new BadRequestException("Account is not ");
    }

    public async Task<ICircularBuffer<ClassifierModel>> GetClassifierStream(Guid shopId)
    {
        // validation is already in shop service
        // TODO [Duy]: Remove this after testing
        // await shopService.GetShopById(shopId);
        return CreateClassifierBufferResult(shopId);
    }

    private ClassifierBuffer CreateClassifierBufferResult(Guid shopId)
    {
        var buffer = new ClassifierBuffer(subject, shopId);
        return buffer;
    }

    public async Task<List<ClassifierModel>> GetClassifierDataForDate(DateOnly date)
    {
        var account = accountService.GetCurrentAccount();
        CheckAuthority(account);
        var shopId = account.ManagingShop!.Id;

        return await GetClassifierDataForShop(date, shopId);
    }

    public async Task<List<ClassifierModel>> GetClassifierDataForDate(Guid shopId, DateOnly date)
    {
        // validation is already in shop service
        // TODO [Duy]: Remove this after testing
        // await shopService.GetShopById(shopId);

        return await GetClassifierDataForShop(date, shopId);
    }

    private async Task<List<ClassifierModel>> GetClassifierDataForShop(DateOnly date, Guid shopId)
    {
        // shopId -> date -> time
        var outputPath = Path.Combine(baseOutputDir, shopId.ToString("N"), date.ToPathString());
        var lines = await File.ReadAllLinesAsync(outputPath);
        return lines.Select(l => JsonSerializer.Deserialize<ClassifierModel>(l)!).ToList();
    }
}
