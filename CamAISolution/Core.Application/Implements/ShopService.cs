using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class ShopService(
    IUnitOfWork unitOfWork,
    IAppLogging<ShopService> logger,
    IBaseMapping mapping,
    IAccountService accountService
) : IShopService
{
    public async Task<Shop> CreateShop(CreateOrUpdateShopDto shopDto)
    {
        await IsValidShopDto(shopDto);
        var shop = mapping.Map<CreateOrUpdateShopDto, Shop>(shopDto);
        shop.ShopStatusId = ShopStatusEnum.Active;
        shop = await unitOfWork.Shops.AddAsync(shop);
        await unitOfWork.CompleteAsync();
        return shop;
    }

    public Task DeleteShop(Guid id)
    {
        logger.Info($"{nameof(DeleteShop)} was not Implemented");
        throw new ServiceUnavailableException("");
    }

    public async Task<Shop> GetShopById(Guid id)
    {
        var foundShop = await unitOfWork.Shops.GetAsync(new ShopByIdRepoSpec(id));
        if (foundShop.Values.Count == 0)
            throw new NotFoundException(typeof(Shop), id);
        return foundShop.Values[0];
    }

    public async Task<PaginationResult<Shop>> GetShops(ShopSearchRequest searchRequest)
    {
        var shops = await unitOfWork.Shops.GetAsync(new ShopSearchSpec(searchRequest));
        return shops;
    }

    public async Task<Shop> UpdateShop(Guid id, CreateOrUpdateShopDto shopDto)
    {
        var foundShops = await unitOfWork.Shops.GetAsync(new ShopByIdRepoSpec(id, false));
        if (foundShops.Values.Count == 0)
            throw new NotFoundException(typeof(Shop), id);
        var foundShop = foundShops.Values[0];
        if (foundShop.ShopStatusId == ShopStatusEnum.Inactive && !await AreRequiredRolesMatched(RoleEnum.Admin))
            throw new BadRequestException("Cannot modified inactive shop");
        await IsValidShopDto(shopDto);
        mapping.Map(shopDto, foundShop);
        await unitOfWork.CompleteAsync();
        return await GetShopById(id);
    }

    public async Task<Shop> UpdateStatus(Guid shopId, int shopStatusId)
    {
        var foundShop = await unitOfWork.Shops.GetByIdAsync(shopId);
        if (foundShop == null)
            throw new NotFoundException(typeof(Shop), shopId);

        //Check if current user is not a shop manager or a brand manager of this shop and alse not an admin, then reject the action.
        var currentAccount = await accountService.GetCurrentAccount();
        var isCurrentShopManager = foundShop.ShopManagerId == currentAccount.Id;
        var isCurrentBrandManager = currentAccount.Brand != null && foundShop.BrandId == currentAccount.Brand.Id;
        var isAdmin = await AreRequiredRolesMatched(RoleEnum.Admin);
        if ((isCurrentShopManager || isCurrentBrandManager) && !isAdmin)
            throw new ForbiddenException("Current user not allowed to do this action.");

        if (foundShop.ShopStatusId == ShopStatusEnum.Inactive && !await AreRequiredRolesMatched(RoleEnum.Admin))
            throw new BadRequestException($"Cannot update inactive shop");
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
    private async Task<bool> AreRequiredRolesMatched(params int[] roles)
    {
        var account = await accountService.GetCurrentAccount();
        return account.Roles.Select(r => r.Id).Intersect(roles).Any();
    }

    private async Task IsValidShopDto(CreateOrUpdateShopDto shopDto)
    {
        var isFoundWard = await unitOfWork.Wards.IsExisted(shopDto.WardId);
        if (!isFoundWard)
            throw new NotFoundException(typeof(Ward), shopDto.WardId);
        var foundBrand = await unitOfWork.Brands.GetByIdAsync(shopDto.BrandId);
        if (foundBrand is { BrandStatusId: BrandStatusEnum.Inactive })
        {
            logger.Error($"Found Brand is {nameof(BrandStatusEnum.Inactive)}. Cannot updated");
            throw new NotFoundException(typeof(Brand), shopDto.BrandId);
        }
    }

    /// <summary>
    /// Get Shops base on current user's roles.
    /// </summary>
    /// <param name="searchShopDto">Filtering</param>
    /// <returns><see cref="PaginationResult{Shop}"/> </returns>
    public async Task<PaginationResult<Shop>> GetCurrentAccountShops(ShopSearchRequest searchRequest)
    {
        if (await AreRequiredRolesMatched(RoleEnum.Admin))
            return await GetShops(searchRequest);

        var currentAccount = await accountService.GetCurrentAccount();
        if (await AreRequiredRolesMatched(RoleEnum.BrandManager))
        {
            if (currentAccount.Brand == null)
                return new PaginationResult<Shop>();
            else
            {
                searchRequest.BrandId = currentAccount.Brand.Id;
                return await GetShops(searchRequest);
            }
        }

        if (await AreRequiredRolesMatched(RoleEnum.ShopManager))
        {
            searchRequest.ShopManagerId = currentAccount.Id;
            return await GetShops(searchRequest);
        }
        return new PaginationResult<Shop>();
    }
}
