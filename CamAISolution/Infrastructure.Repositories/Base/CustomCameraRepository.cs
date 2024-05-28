using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Repositories;
using Core.Domain.Specifications.Repositories;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base;

public class CustomCameraRepository(
    CamAIContext context,
    IRepositorySpecificationEvaluator<Camera> specificationEvaluator
) : Repository<Camera>(context, specificationEvaluator), ICustomCameraRepository
{
    public void DisconnectCamerasForShop(Guid shopId)
    {
        Context
            .Set<Camera>()
            .Where(x => x.ShopId == shopId && x.Status == CameraStatus.Connected)
            .ExecuteUpdate(x => x.SetProperty(c => c.Status, CameraStatus.Disconnected));
    }
}
