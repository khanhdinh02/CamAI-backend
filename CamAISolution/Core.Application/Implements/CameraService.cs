using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class CameraService(
    IAccountService accountService,
    IShopService shopService,
    IUnitOfWork unitOfWork,
    IBaseMapping mapper
) : ICameraService
{
    public async Task<PaginationResult<Camera>> GetCameras(Guid shopId)
    {
        // get shop to validate role
        await shopService.GetShopById(shopId);
        return await unitOfWork.Cameras.GetAsync(
            x => x.ShopId == shopId && x.Status != CameraStatus.Disabled,
            takeAll: true
        );
    }

    public async Task<PaginationResult<Camera>> GetCamerasForEdgeBox(Guid shopId)
    {
        // get shop to validate role
        return await unitOfWork.Cameras.GetAsync(
            x => x.ShopId == shopId && x.Status != CameraStatus.Disabled,
            takeAll: true
        );
    }

    public async Task<Camera> GetCameraById(Guid id)
    {
        var camera =
            (
                await unitOfWork.Cameras.GetAsync(
                    x => x.Id == id && x.Status != CameraStatus.Disabled,
                    includeProperties: ["Shop"]
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

    public async Task<Camera> UpsertCamera(Camera camera)
    {
        var foundCamera = await unitOfWork.Cameras.GetByIdAsync(camera.Id);

        if (foundCamera == null)
            await unitOfWork.Cameras.AddAsync(camera);
        else
        {
            mapper.Map(camera, foundCamera);
            unitOfWork.Cameras.Update(foundCamera);
        }

        await unitOfWork.CompleteAsync();
        return camera;
    }

    public async Task DeleteCamera(Guid id)
    {
        var camera = await unitOfWork.Cameras.GetByIdAsync(id);
        if (camera == null)
            return;

        var hasRelatedEntity = (await unitOfWork.Evidences.GetAsync(x => x.CameraId == id)).IsValuesEmpty;
        if (hasRelatedEntity)
        {
            camera.Status = CameraStatus.Disabled;
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
