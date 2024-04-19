using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Repositories;
using Core.Domain.Specifications.Repositories;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base;

public class CustomEdgeBoxInstallRepository(
    CamAIContext context,
    IRepositorySpecificationEvaluator<EdgeBoxInstall> specificationEvaluator
) : Repository<EdgeBoxInstall>(context, specificationEvaluator), ICustomEdgeBoxInstallRepository
{
    public void UpdateStatusBy(EdgeBoxInstallStatus status, Expression<Func<EdgeBoxInstall, bool>> expr)
    {
        Context
            .Set<EdgeBoxInstall>()
            .Where(expr)
            .ExecuteUpdate(x => x.SetProperty(eb => eb.EdgeBoxInstallStatus, status));
    }
}
