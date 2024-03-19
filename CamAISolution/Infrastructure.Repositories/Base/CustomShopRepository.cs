using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Repositories;
using Core.Domain.Specifications.Repositories;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base;

public class CustomShopRepository(CamAIContext context, IRepositorySpecificationEvaluator<Shop> specificationEvaluator)
    : Repository<Shop>(context, specificationEvaluator),
        ICustomShopRepository
{
    public void UpdateStatusInBrand(Guid brandId, ShopStatus status)
    {
        Context
            .Set<Shop>()
            .Where(x => x.BrandId == brandId)
            .ExecuteUpdate(x => x.SetProperty(s => s.ShopStatus, status));
    }
}
