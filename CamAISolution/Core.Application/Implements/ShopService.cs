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
        await IsValidShopDto(shopDto);
        var shop = mapping.Map<CreateOrUpdateShopDto, Shop>(shopDto);
        shop.ShopStatus = ShopStatus.Active;
        var account = accountService.GetCurrentAccount();
        if (!account.BrandId.HasValue)
            throw new BadRequestException(
                $"Cannot create shop because current account doesn't have any registered Brand"
            );
        shop.BrandId = account.BrandId.Value;
        shop = await unitOfWork.Shops.AddAsync(shop);
        await unitOfWork.CompleteAsync();
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

    public async Task<PaginationResult<Shop>> GetShopsInstallingEdgeBox(bool hasEdgeBoxInstalling)
    {
        var installingEdgeBoxShops = (
            await unitOfWork.Shops.GetAsync(
                s =>
                    s.EdgeBoxInstalls.Any(i => i.EdgeBoxInstallStatus != EdgeBoxInstallStatus.Disabled)
                    == hasEdgeBoxInstalling,
                takeAll: true
            )
        ).Values.ToList();
        return new PaginationResult<Shop>
        {
            Values = installingEdgeBoxShops,
            PageIndex = 0,
            PageSize = installingEdgeBoxShops.Count,
            TotalCount = installingEdgeBoxShops.Count
        };
    }

    public async Task<Shop> UpdateShop(Guid id, CreateOrUpdateShopDto shopDto)
    {
        var foundShops = await unitOfWork.Shops.GetAsync(new ShopByIdRepoSpec(id, false));
        if (foundShops.IsValuesEmpty)
            throw new NotFoundException(typeof(Shop), id);
        var foundShop = foundShops.Values[0];
        var currentAccount = accountService.GetCurrentAccount();
        if (!(currentAccount.BrandId.HasValue && foundShop.BrandId == currentAccount.BrandId.Value))
            throw new ForbiddenException("Current user not allowed to do this action.");
        if (foundShop.ShopStatus != ShopStatus.Active)
            throw new BadRequestException($"Cannot modified inactive shop");
        await IsValidShopDto(shopDto, id);
        mapping.Map(shopDto, foundShop);
        var shop = unitOfWork.Shops.Update(foundShop);
        if (await unitOfWork.CompleteAsync() > 0)
        {
            try
            {
                eventManager.NotifyShopChanged(shop);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
        return shop;
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
        if (shopDto.ShopManagerId.HasValue)
        {
            var foundAccounts = await unitOfWork.Accounts.GetAsync(
                expression: a =>
                    a.Id == shopDto.ShopManagerId.Value
                    && (a.AccountStatus == AccountStatus.Active || a.AccountStatus == AccountStatus.New),
                includeProperties: [nameof(Account.ManagingShop)]
            );
            if (foundAccounts.Values.Count == 0)
                throw new NotFoundException(typeof(Account), shopDto.ShopManagerId.Value);
            var account = foundAccounts.Values[0];
            if (account.Role != Role.ShopManager)
                throw new BadRequestException("Account is not a shop manager");
            var brandManager = accountService.GetCurrentAccount();

            if (account.ManagingShop != null && account.ManagingShop?.Id != shopId)
                throw new BadRequestException("Account is a manager of another shop");

            if (account.BrandId != brandManager.BrandId)
                throw new BadRequestException(
                    $"Account is not in the same brand as brand manager. Id {brandManager.BrandId}"
                );
        }
    }
}
