using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application;

public class ShopService(IUnitOfWork unitOfWork, IAppLogging<ShopService> logger, IBaseMapping mapping) : IShopService
{
    public async Task<Shop> CreateShop(Shop shop)
    {
        var isFoundWard = await unitOfWork.Wards.IsExisted(shop.WardId);
        if (!isFoundWard)
            throw new NotFoundException(typeof(Ward), shop.WardId);
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

    public async Task<PaginationResult<Shop>> GetShops(SearchShopRequest searchRequest)
    {
        var shops = await unitOfWork.Shops.GetAsync(new SearchShopSpec(searchRequest));
        return shops;
    }

    public async Task<Shop> UpdateShop(Guid id, CreateOrUpdateShopDto shopDto)
    {
        var foundShops = await unitOfWork.Shops.GetAsync(new ShopByIdRepoSpec(id, false));
        if (foundShops.Values.Count == 0)
            throw new NotFoundException(typeof(Shop), id);
        var foundShop = foundShops.Values[0];
        //TODO: If current actor has role Admin, allow to update inactive shop
        if (foundShop.ShopStatusId == BrandStatusEnum.Inactive)
            throw new BadRequestException("Cannot modified inactive shop");
        if (!await unitOfWork.Wards.IsExisted(shopDto.WardId))
            throw new NotFoundException(typeof(Ward), shopDto.WardId);

        mapping.Map(shopDto, foundShop);

        await unitOfWork.CompleteAsync();
        return await GetShopById(id);
    }

    public async Task<Shop> UpdateStatus(Guid shopId, Guid shopStatusId)
    {
        var foundShop = await unitOfWork.Shops.GetByIdAsync(shopId);
        if (foundShop == null)
            throw new NotFoundException(typeof(Shop), shopId);
        //TODO: If current actor has role Admin, allow to update inactive shop
        if (foundShop.ShopStatusId == ShopStatusEnum.Inactive)
            throw new BadRequestException($"Cannot update inactive shop");
        foundShop.ShopStatusId = shopStatusId;
        await unitOfWork.CompleteAsync();
        return await GetShopById(shopId);
    }
}
