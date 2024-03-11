using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Repositories;
using Core.Domain.Specifications.Repositories;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base;

public class CustomAccountRepository(
    CamAIContext context,
    IRepositorySpecificationEvaluator<Account> specificationEvaluator
) : Repository<Account>(context, specificationEvaluator), ICustomAccountRepository
{
    public void UpdateStatusInBrand(Guid brandId, AccountStatus status)
    {
        Context
            .Set<Account>()
            .Where(x => x.BrandId == brandId)
            .ExecuteUpdate(x => x.SetProperty(s => s.AccountStatus, status));
    }
}
