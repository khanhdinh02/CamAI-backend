using Core.Application.Events;
using Core.Application.Exceptions;
using Core.Application.Models;
using Core.Domain.DTO;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Models.Consumers;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class ReportService(IAccountService accountService, IShopService shopService, ClassifierSubject subject)
    : IReportService
{
    public Task<ICircularBuffer<ClassifierModel>> GetClassifierStream()
    {
        var account = accountService.GetCurrentAccount();
        if (!account.HasRole(RoleEnum.ShopManager))
            throw new BadRequestException("Please specify a shop id");
        if (account.ManagingShop == null)
            throw new BadRequestException("Account is not ");

        var buffer = CreateClassifierBufferResult(account.ManagingShop.Id);
        return Task.FromResult<ICircularBuffer<ClassifierModel>>(buffer);
    }

    public async Task<ICircularBuffer<ClassifierModel>> GetClassifierStream(Guid shopId)
    {
        // validation is already in shop service
        await shopService.GetShopById(shopId);
        return CreateClassifierBufferResult(shopId);
    }

    private ClassifierBuffer CreateClassifierBufferResult(Guid shopId)
    {
        var buffer = new ClassifierBuffer(subject, shopId);
        return buffer;
    }
}
