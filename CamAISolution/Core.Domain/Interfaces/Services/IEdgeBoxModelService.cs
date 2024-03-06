using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Services;

public interface IEdgeBoxModelService
{
    Task<IEnumerable<EdgeBoxModel>> GetAll();
}
