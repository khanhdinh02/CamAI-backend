using System.Runtime.InteropServices.Marshalling;
using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Core.Domain.Interfaces.Specifications.Repositories;
using Core.Domain.Models;

namespace Core.Application;

public class ShopService(IUnitOfWork unitOfWork, IAppLogging<ShopService> logger) : IShopService
{
    public async Task<Shop> CreateShop(Shop shop)
    {
        var findWard = await unitOfWork.Wards.GetAsync(expression: new WardByIdSepcification(shop.WardId).GetExpression());
        if (findWard.Values.Count == 0)
            throw new NotFoundException(typeof(Ward), shop.WardId, GetType());
        shop.Status = Shop.Statuses.Active;
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
        var find = await unitOfWork.Shops.GetAsync(new ShopByIdRepoSpecfication(id));
        if (find.Values.Count == 0)
            throw new NotFoundException(typeof(Shop), id, GetType());
        return find.Values.First();
    }

    public async Task<PaginationResult<Shop>> GetShops(SearchShopRequest searchRequest)
    {
        var shops = await unitOfWork.Shops.GetAsync(new SearchShopSpecification(searchRequest));
        return shops;
    }

    public Task<Shop> UpdateShop()
    {
        throw new NotImplementedException();
    }
}
