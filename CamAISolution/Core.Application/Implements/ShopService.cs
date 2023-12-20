using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Core.Domain.Models;

namespace Core.Application;

public class ShopService(IUnitOfWork unitOfWork, IAppLogging<ShopService> logger) : IShopService
{
    public async Task<Shop> CreateShop(Shop shop)
    {
        var isFoundWard = await unitOfWork.Wards.IsExisted(shop.WardId);
        if (!isFoundWard)
            throw new NotFoundException(typeof(Ward), shop.WardId, GetType());
        shop.ShopStatusId = AppConstant.ShopActiveStatus;
        shop = await unitOfWork.Shops.AddAsync(shop);
        await unitOfWork.CompleteAsync();
        logger.Info($"New shop: {System.Text.Json.JsonSerializer.Serialize(shop)}");
        return shop;
    }

    public Task DeleteShop(Guid id)
    {
        logger.Info($"{nameof(this.DeleteShop)} was not Implemented");
        throw new ServiceUnavailableException("");
    }

    public async Task<Shop> GetShopById(Guid id)
    {
        var foundShop = await unitOfWork.Shops.GetAsync(new ShopByIdRepoSpecfication(id));
        if (foundShop.Values.Count == 0)
            throw new NotFoundException(typeof(Shop), id, GetType());
        return foundShop.Values.First();
    }

    public async Task<PaginationResult<Shop>> GetShops(SearchShopRequest searchRequest)
    {
        var shops = await unitOfWork.Shops.GetAsync(new SearchShopSpecification(searchRequest));
        return shops;
    }

    //TODO: Create Mapper interface and inject to map
    public async Task<Shop> UpdateShop(Guid id, UpdateShopDto shopDto)
    {
        var foundShop = await unitOfWork.Shops.GetByIdAsync(id);
        if (foundShop is null)
            throw new NotFoundException(typeof(Shop), id, GetType());
        if (foundShop.ShopStatusId == AppConstant.ShopInactiveStatus)
            throw new BadRequestException($"Cannot modified inactive shop");
        foundShop.Name = shopDto.Name ?? foundShop.Name;
        foundShop.AddressLine = shopDto.AddressLine ?? foundShop.AddressLine;
        foundShop.Phone = foundShop.Phone;
        if (shopDto.WardId.HasValue)
        {
            var isFoundWard = await unitOfWork.Wards.IsExisted(shopDto.WardId.Value);
            if (!isFoundWard)
                throw new NotFoundException(typeof(Ward), shopDto.WardId, GetType());
            foundShop.WardId = shopDto.WardId.Value;
        }
        if (shopDto.Status.HasValue)
        {
            var isFoundStatus = await unitOfWork.ShopStatuses.IsExisted(shopDto.Status.Value);
            if (!isFoundStatus)
                throw new NotFoundException(typeof(ShopStatus), shopDto.Status.Value, GetType());
            foundShop.ShopStatusId = shopDto.Status.Value;
        }
        await unitOfWork.CompleteAsync();
        return foundShop;
    }
}
