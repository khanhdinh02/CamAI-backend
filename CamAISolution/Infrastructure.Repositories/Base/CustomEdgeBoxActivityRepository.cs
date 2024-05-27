using Core.Domain.Entities;
using Core.Domain.Repositories;
using Core.Domain.Specifications.Repositories;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base;

public class CustomEdgeBoxActivityRepository(
    CamAIContext context,
    IRepositorySpecificationEvaluator<EdgeBoxActivity> specificationEvaluator
) : Repository<EdgeBoxActivity>(context, specificationEvaluator), ICustomEdgeBoxActivityRepository
{
    public void DeleteActivityByEdgeBoxId(Guid edgeBoxId)
    {
        Context.Set<EdgeBoxActivity>().Where(x => x.EdgeBoxId == edgeBoxId).ExecuteDelete();
    }
}
