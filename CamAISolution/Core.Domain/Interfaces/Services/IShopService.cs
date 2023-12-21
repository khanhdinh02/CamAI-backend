using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Domain.Models.DTOs.Shops;

namespace Core.Domain.Interfaces.Services;

public interface IShopService
{
    Task<PaginationResult<Shop>> GetShops(SearchShopRequest searchRequest);
    Task<Shop> GetShopById(Guid id);
    Task<Shop> CreateShop(Shop shop);
    Task<Shop> UpdateShop(Guid id, UpdateShopDto shopDto);
    Task DeleteShop(Guid id);
}
