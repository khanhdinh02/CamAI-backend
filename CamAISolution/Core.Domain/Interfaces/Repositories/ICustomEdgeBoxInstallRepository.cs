using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Domain.Repositories;

public interface ICustomEdgeBoxInstallRepository : IRepository<EdgeBoxInstall>
{
    void UpdateStatusBy(EdgeBoxInstallStatus status, Expression<Func<EdgeBoxInstall, bool>> expr);
}
