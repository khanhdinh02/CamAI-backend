using Core.Application.Events;
using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Application.Specifications.Shops.Repositories;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class ShopService(
    IUnitOfWork unitOfWork,
    IEdgeBoxService edgeBoxService,
    IAppLogging<ShopService> logger,
    IBaseMapping mapping,
    IAccountService accountService,
    EventManager eventManager
) : IShopService
{
    public async Task<Shop> CreateShop(CreateOrUpdateShopDto shopDto)
    {
        await unitOfWork.BeginTransaction();

        var account = accountService.GetCurrentAccount();
        if (!account.BrandId.HasValue)
            throw new BadRequestException(
                "Cannot create shop because current account doesn't have any registered Brand"
            );
        await IsValidShopDto(shopDto);

        var shop = mapping.Map<CreateOrUpdateShopDto, Shop>(shopDto);
        shop.ShopManagerId = null;
        shop.ShopStatus = ShopStatus.Active;
        shop.BrandId = account.BrandId.Value;
        shop = await unitOfWork.Shops.AddAsync(shop);
        await AssignShopManager(shop, shopDto.ShopManagerId);

        await unitOfWork.CommitTransaction();
        return shop;
    }

    public async Task DeleteShop(Guid id)
    {
        var installedEdgeBoxes = await edgeBoxService.GetEdgeBoxesByShop(id);
        if (installedEdgeBoxes.Any())
            throw new BadRequestException("Cannot delete shop that has active edge box");

        var shop = await GetShopById(id);
        if (
            (await unitOfWork.EdgeBoxInstalls.GetAsync(x => x.ShopId == id, takeAll: true)).Values is
            { Count: > 0 } installs
        )
        {
            if (installs.Any(i => i.EdgeBoxInstallStatus != EdgeBoxInstallStatus.Disabled))
                throw new BadRequestException("Cannot delete shop that currently has installed edge boxes");
            shop.ShopStatus = ShopStatus.Inactive;
        }
        else
        {
            unitOfWork.Shops.Delete(shop);
            unitOfWork.Employees.DeleteEmployeeInShop(id);
        }
        await unitOfWork.CompleteAsync();
        logger.Info($"Shop{shop.Id} has been Inactivated");
    }

    public async Task<Shop> GetShopById(Guid id)
    {
        var foundShop = await unitOfWork.Shops.GetAsync(new ShopByIdRepoSpec(id));
        var notFoundException = new NotFoundException(typeof(Shop), id);
        if (foundShop.Values.Count == 0)
            throw notFoundException;
        var account = accountService.GetCurrentAccount();
        var shop = foundShop.Values[0];
        if (account.Role == Role.Admin)
            return shop;
        if (account.Role == Role.BrandManager)
            return account.Brand != null && shop.BrandId == account.Brand.Id ? shop : throw notFoundException;
        if (account.Role == Role.ShopManager && shop.ShopManagerId == account.Id)
            return shop;
        throw notFoundException;
    }

    public async Task<PaginationResult<Shop>> GetShops(ShopSearchRequest searchRequest)
    {
        var includeWard = false;
        var account = accountService.GetCurrentAccount();
        if (account.Role == Role.BrandManager)
            searchRequest.BrandId = account.BrandId;
        else if (account.Role == Role.ShopManager)
        {
            searchRequest.ShopManagerId = account.Id;
            searchRequest.Status = ShopStatus.Active;
            includeWard = true;
        }

        var shops = await unitOfWork.Shops.GetAsync(new ShopSearchSpec(searchRequest, includeWard));
        return shops;
    }

    public async Task<Shop> UpdateShop(Guid id, CreateOrUpdateShopDto shopDto)
    {
        var foundShop =
            (await unitOfWork.Shops.GetAsync(new ShopByIdRepoSpec(id, false))).Values.FirstOrDefault()
            ?? throw new NotFoundException(typeof(Shop), id);
        var currentAccount = accountService.GetCurrentAccount();
        if (!(currentAccount.BrandId.HasValue && foundShop.BrandId == currentAccount.BrandId.Value))
            throw new ForbiddenException("Current user not allowed to do this action.");
        if (foundShop.ShopStatus != ShopStatus.Active)
            throw new BadRequestException("Cannot modified inactive shop");
        await IsValidShopDto(shopDto, id);

        var oldShopManagerId = foundShop.ShopManagerId;
        mapping.Map(shopDto, foundShop);
        foundShop.ShopManagerId = oldShopManagerId;
        await AssignShopManager(foundShop, shopDto.ShopManagerId);
        eventManager.NotifyShopChanged(foundShop);
        return foundShop;
    }

    public async Task<Shop> UpdateShopStatus(Guid shopId, ShopStatus shopStatus)
    {
        var foundShop = await unitOfWork.Shops.GetByIdAsync(shopId);
        if (foundShop == null)
            throw new NotFoundException(typeof(Shop), shopId);

        var currentAccount = accountService.GetCurrentAccount();
        var isBrandManager = currentAccount.BrandId.HasValue && foundShop.BrandId == currentAccount.BrandId.Value;
        if (!isBrandManager)
            throw new ForbiddenException("Current user not allowed to do this action.");
        // If shop's status is Inactive, only admin can update status
        if (foundShop.ShopStatus != ShopStatus.Active && currentAccount.Role != Role.Admin)
            throw new BadRequestException("Cannot update inactive shop");
        foundShop.ShopStatus = shopStatus;
        await unitOfWork.CompleteAsync();
        return await GetShopById(shopId);
    }

    private async Task IsValidShopDto(CreateOrUpdateShopDto shopDto, Guid? shopId = null)
    {
        if (!await unitOfWork.Wards.IsExisted(shopDto.WardId))
            throw new NotFoundException(typeof(Ward), shopDto.WardId);
        var account =
            (
                await unitOfWork.Accounts.GetAsync(
                    expression: a =>
                        a.Id == shopDto.ShopManagerId
                        && (a.AccountStatus == AccountStatus.Active || a.AccountStatus == AccountStatus.New),
                    includeProperties: [nameof(Account.ManagingShop)]
                )
            ).Values.FirstOrDefault() ?? throw new NotFoundException(typeof(Account), shopDto.ShopManagerId);
        if (account.Role != Role.ShopManager)
            throw new BadRequestException("Account is not a shop manager");

        var brandManager = accountService.GetCurrentAccount();
        if (account.BrandId != brandManager.BrandId)
            throw new BadRequestException(
                $"Account is not in the same brand as brand manager. Id {brandManager.BrandId}"
            );
    }

    private async Task AssignShopManager(Shop shop, Guid shopManagerId)
    {
        if (shop.ShopManagerId == shopManagerId)
            return;

        var shopManager =
            (
                await unitOfWork.Accounts.GetAsync(
                    a => a.Id == shopManagerId,
                    includeProperties: [nameof(Account.ManagingShop)]
                )
            ).Values.FirstOrDefault() ?? throw new NotFoundException(typeof(Account), shopManagerId);
        if (shopManager.Role != Role.ShopManager)
            throw new BadRequestException("Account is not a shop manager");

        if (shopManager.ManagingShop != null)
        {
            var oldShop =
                await unitOfWork.Shops.GetByIdAsync(shopManager.ManagingShop.Id)
                ?? throw new NotFoundException(typeof(Shop), shopManager.ManagingShop.Id);
            oldShop.ShopManagerId = null;
            unitOfWork.Shops.Update(oldShop);
        }
        shop.ShopManagerId = shopManagerId;
        unitOfWork.Shops.Update(shop);
        await unitOfWork.CompleteAsync();
    }
}
