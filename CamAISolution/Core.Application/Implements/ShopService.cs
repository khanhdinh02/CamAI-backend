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
            throw new NotFoundException(typeof(Ward), shop.WardId, GetType());
        shop.ShopStatusId = AppConstant.ShopActiveStatus;
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
            throw new NotFoundException(typeof(Shop), id, GetType());
        return foundShop.Values[0];
    }

    public async Task<PaginationResult<Shop>> GetShops(SearchShopRequest searchRequest)
    {
        var shops = await unitOfWork.Shops.GetAsync(new SearchShopSpec(searchRequest));
        return shops;
    }

    public async Task<Shop> UpdateShop(Guid id, UpdateShopDto shopDto)
    {
        var foundShop = await unitOfWork.Shops.GetByIdAsync(id);
        if (foundShop is null)
            throw new NotFoundException(typeof(Shop), id, GetType());
        //TODO: If current acctor has role Admin, allow to update inactive shop
        if (foundShop.ShopStatusId == AppConstant.ShopInactiveStatus)
            throw new BadRequestException($"Cannot modified inactive shop");
        foundShop = mapping.Map(shopDto, foundShop);
        if (shopDto.WardId.HasValue)
        {
            var isFoundWard = await unitOfWork.Wards.IsExisted(shopDto.WardId.Value);
            if (!isFoundWard)
                throw new NotFoundException(typeof(Ward), shopDto.WardId, GetType());
        }
        if (shopDto.Status.HasValue)
        {
            var isFoundStatus = await unitOfWork.ShopStatuses.IsExisted(shopDto.Status.Value);
            if (!isFoundStatus)
                throw new NotFoundException(typeof(ShopStatus), shopDto.Status.Value, GetType());
        }
        await unitOfWork.CompleteAsync();
        return foundShop;
    }
}
