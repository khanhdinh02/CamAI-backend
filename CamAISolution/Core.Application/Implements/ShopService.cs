using Core.Application.Events;
using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Application.Specifications.Shops.Repositories;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class ShopService(
    IUnitOfWork unitOfWork,
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
        shop.ShopStatusId = ShopStatusEnum.Active;
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

    //TODO [Dat]: update shop status if there is EdgeBoxInstall
    public async Task DeleteShop(Guid id)
    {
        var shop = unitOfWork.Shops.Delete(new Shop { Id = id });
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
        if (account.HasRole(RoleEnum.Admin))
            return shop;
        if (account.HasRole(RoleEnum.BrandManager))
            return account.Brand != null && shop.BrandId == account.Brand.Id ? shop : throw notFoundException;
        if (account.HasRole(RoleEnum.ShopManager) && shop.ShopManagerId == account.Id)
            return shop;
        throw notFoundException;
    }

    public async Task<PaginationResult<Shop>> GetShops(ShopSearchRequest searchRequest)
    {
        var account = accountService.GetCurrentAccount();
        if (account.HasRole(RoleEnum.Admin))
        {
            // this if will ensure that admin role has highest priority
        }
        else if (account.HasRole(RoleEnum.BrandManager))
            searchRequest.BrandId = account.BrandId;
        else if (account.HasRole(RoleEnum.ShopManager))
        {
            searchRequest.ShopManagerId = account.Id;
            // TODO [Duy]: is there a way to specify not condition like != Status.Inactive
            searchRequest.StatusId = ShopStatusEnum.Active;
        }

        var shops = await unitOfWork.Shops.GetAsync(new ShopSearchSpec(searchRequest));
        return shops;
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
        if (foundShop.ShopStatusId != ShopStatusEnum.Active)
            throw new BadRequestException($"Cannot modified inactive shop");
        await IsValidShopDto(shopDto, id);
        mapping.Map(shopDto, foundShop);
        await unitOfWork.CompleteAsync();
        var shop = await GetShopById(id);

        eventManager.NotifyShopChanged(shop);
        return shop;
    }

    public async Task<Shop> UpdateShopStatus(Guid shopId, int shopStatusId)
    {
        var foundShop = await unitOfWork.Shops.GetByIdAsync(shopId);
        if (foundShop == null)
            throw new NotFoundException(typeof(Shop), shopId);

        //Check if current user is not a shop manager or a brand manager of this shop and else not an admin, then reject the action.
        var currentAccount = accountService.GetCurrentAccount();
        var isShopManager = foundShop.ShopManagerId == currentAccount.Id;
        var isBrandManager = currentAccount.BrandId.HasValue && foundShop.BrandId == currentAccount.BrandId.Value;
        var isAdmin = AreRequiredRolesMatched(RoleEnum.Admin);
        if ((isShopManager || isBrandManager) && !isAdmin)
            throw new ForbiddenException("Current user not allowed to do this action.");

        if (foundShop.ShopStatusId != ShopStatusEnum.Active && !isAdmin)
            throw new BadRequestException("Cannot update inactive shop");
        foundShop.ShopStatusId = shopStatusId;
        await unitOfWork.CompleteAsync();
        return await GetShopById(shopId);
    }

    //TODO [Dat]: use role service instead

    /// <summary>
    /// Check if input roles match with current user's roles
    /// </summary>
    /// <param name="roles">List of role IDs. Use RoleEnum.cs to get the constant role id</param>
    /// <returns></returns>
    private bool AreRequiredRolesMatched(params int[] roles)
    {
        var account = accountService.GetCurrentAccount();
        return account.Roles.Select(r => r.Id).Intersect(roles).Any();
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
                    && (a.AccountStatusId == AccountStatusEnum.Active || a.AccountStatusId == AccountStatusEnum.New),
                includeProperties: [nameof(Account.Roles), nameof(Account.ManagingShop)]
            );
            if (foundAccounts.Values.Count == 0)
                throw new NotFoundException(typeof(Account), shopDto.ShopManagerId.Value);
            var account = foundAccounts.Values[0];
            if (!account.HasRole(RoleEnum.ShopManager))
                throw new BadRequestException("Account is not a shop manager");
            var brandManager = accountService.GetCurrentAccount();

            if (account.ManagingShop?.Id != shopId)
                throw new BadRequestException("Account is a manager of another shop");

            if (account.BrandId != brandManager.BrandId)
                throw new BadRequestException(
                    $"Account is not in the same brand as brand manager. Id {brandManager.BrandId}"
                );
        }
    }
}
