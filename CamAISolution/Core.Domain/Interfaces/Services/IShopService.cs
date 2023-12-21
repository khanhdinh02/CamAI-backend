using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Services;

public interface IShopService
{
    Task<PaginationResult<Shop>> GetShops(SearchShopRequest searchRequest);
    Task<Shop> GetShopById(Guid id);
    Task<Shop> CreateShop(Shop shop);
    Task<Shop> UpdateShop(Guid id, UpdateShopDto shopDto);
    Task DeleteShop(Guid id);
}
