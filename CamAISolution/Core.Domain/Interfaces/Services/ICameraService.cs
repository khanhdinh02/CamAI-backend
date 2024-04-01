using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Interfaces.Services;

public interface ICameraService
{
    Task<PaginationResult<Camera>> GetCameras(Guid shopId);
    Task<Camera> GetCameraById(Guid id);
    Task<Camera> UpsertCamera(Camera camera);
    Task DeleteCamera(Guid id);
    Task CreateCameraIfNotExist(Guid id, Guid shopId);
}
