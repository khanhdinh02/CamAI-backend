using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class CameraService(IAccountService accountService, IShopService shopService, IUnitOfWork unitOfWork)
    : ICameraService
{
    public async Task<PaginationResult<Camera>> GetCameras(Guid shopId)
    {
        // get shop to validate role
        await shopService.GetShopById(shopId);
        return await unitOfWork.Cameras.GetAsync(
            x => x.ShopId == shopId && x.Status == CameraStatus.Active,
            takeAll: true
        );
    }

    public async Task<Camera> GetCameraById(Guid id)
    {
        var camera =
            (
                await unitOfWork.Cameras.GetAsync(
                    x => x.Id == id && x.Status == CameraStatus.Active,
                    includeProperties: ["shop"]
                )
            ).Values.FirstOrDefault() ?? throw new NotFoundException(typeof(Camera), id);

        var currentAccount = accountService.GetCurrentAccount();
        switch (currentAccount.Role)
        {
            case Role.ShopManager when camera.ShopId != currentAccount.ManagingShop!.Id:
            case Role.BrandManager when camera.Shop.BrandId != currentAccount.BrandId:
                throw new NotFoundException(typeof(Camera), id);
        }

        return camera;
    }

    public async Task<Camera> UpsertCameraForRoleEdgeBox(Camera camera)
    {
        var foundCamera = await unitOfWork.Cameras.GetByIdAsync(camera.Id);

        if (foundCamera == null)
            await unitOfWork.Cameras.AddAsync(camera);
        else
        {
            foundCamera.Name = camera.Name;
            foundCamera.Zone = camera.Zone;
            unitOfWork.Cameras.Update(foundCamera);
        }

        await unitOfWork.CompleteAsync();
        return camera;
    }

    public async Task DeleteCameraForRoleEdgeBox(Guid id)
    {
        var camera = await unitOfWork.Cameras.GetByIdAsync(id);
        if (camera == null)
            return;

        var hasRelatedEntity = (await unitOfWork.Evidences.GetAsync(x => x.CameraId == id)).IsValuesEmpty;
        if (hasRelatedEntity)
        {
            camera.Status = CameraStatus.Inactive;
            unitOfWork.Cameras.Update(camera);
        }
        else
            unitOfWork.Cameras.Delete(camera);

        await unitOfWork.CompleteAsync();
    }

    public async Task CreateCameraIfNotExist(Guid id, Guid shopId)
    {
        if (await unitOfWork.Cameras.IsExisted(id))
            return;

        var camera = new Camera
        {
            Id = id,
            ShopId = shopId,
            Name = id.ToString("N")
        };
        await unitOfWork.Cameras.AddAsync(camera);
        await unitOfWork.CompleteAsync();
    }
}
