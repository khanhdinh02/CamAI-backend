using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Services;

public interface IShopService
{
    Task<PaginationResult<Shop>> GetShops(ShopSearchRequest searchRequest);
    Task<Shop> GetShopById(Guid id);
    Task<Shop> CreateShop(CreateOrUpdateShopDto shopDto);
    Task<Shop> UpdateShop(Guid id, CreateOrUpdateShopDto shopDto);
    Task DeleteShop(Guid id);
    Task<Shop> UpdateShopStatus(Guid shopId, int shopStatusId);
}
