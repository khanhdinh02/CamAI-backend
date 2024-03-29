using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Models;

namespace Core.Domain.Services;

public interface IShopService
{
    /// <summary>
    /// Get Shops base on current user's roles.
    /// </summary>
    /// <param name="searchRequest">Filtering</param>
    /// <returns><see cref="PaginationResult{Shop}"/> </returns>
    Task<PaginationResult<Shop>> GetShops(ShopSearchRequest searchRequest);
    Task<Shop> GetShopById(Guid id);
    Task<Shop> CreateShop(CreateOrUpdateShopDto shopDto);
    Task<Shop> UpdateShop(Guid id, CreateOrUpdateShopDto shopDto);
    Task DeleteShop(Guid id);
    Task<Shop> UpdateShopStatus(Guid shopId, ShopStatus shopStatus);
}
