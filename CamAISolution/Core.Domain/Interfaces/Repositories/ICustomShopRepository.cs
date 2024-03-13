using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Domain.Repositories;

public interface ICustomShopRepository : IRepository<Shop>
{
    void UpdateStatusInBrand(Guid brandId, ShopStatus status);
}
