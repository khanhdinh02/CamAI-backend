using Core.Domain.Entities;

namespace Core.Domain.Repositories;

public interface ICustomCameraRepository : IRepository<Camera>
{
    void DisconnectCamerasForShop(Guid shopId);
}
