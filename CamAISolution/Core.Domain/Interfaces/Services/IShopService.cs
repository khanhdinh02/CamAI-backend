using System.Data.Common;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Specifications.Repositories;
using Core.Domain.Models;

namespace Core.Domain;

public interface IShopService
{
    Task<PaginationResult<Shop>> GetShops(SearchShopRequest searchRequest);
    Task<Shop> GetShopById(Guid id);
    Task<Shop> CreateShop(Shop shop);
    Task<Shop> UpdateShop(Guid id, UpdateShopDto shopDto);
    Task DeleteShop(Guid id);
}
