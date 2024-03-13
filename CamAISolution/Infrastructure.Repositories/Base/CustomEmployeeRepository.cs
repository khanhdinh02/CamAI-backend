using Core.Domain.Entities;
using Core.Domain.Repositories;
using Core.Domain.Specifications.Repositories;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base;

public class CustomEmployeeRepository(
    CamAIContext context,
    IRepositorySpecificationEvaluator<Employee> specificationEvaluator
) : Repository<Employee>(context, specificationEvaluator), ICustomEmployeeRepository
{
    public void DeleteEmployeeInShop(Guid shopId)
    {
        Context.Set<Employee>().Where(x => x.ShopId == shopId).ExecuteDelete();
    }
}
