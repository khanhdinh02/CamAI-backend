using Core.Domain.Entities;

namespace Core.Domain.Repositories;

public interface ICustomEdgeBoxActivityRepository : IRepository<EdgeBoxActivity>
{
    void DeleteActivityByEdgeBoxId(Guid edgeBoxId);
}
