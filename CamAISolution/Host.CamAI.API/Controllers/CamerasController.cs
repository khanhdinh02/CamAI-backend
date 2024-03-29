using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CamerasController(ICameraService cameraService, IStreamingService streamingService, IBaseMapping mapping)
    : ControllerBase
{
    [HttpGet("/api/shops/{shopId}/cameras")]
    public async Task<PaginationResult<CameraDto>> GetCameras([FromRoute] Guid shopId)
    {
        return mapping.Map<Camera, CameraDto>(await cameraService.GetCameras(shopId));
    }

    [HttpGet("{id}")]
    public async Task<CameraDto> GetCameraById([FromRoute] Guid id)
    {
        return mapping.Map<Camera, CameraDto>(await cameraService.GetCameraById(id));
    }

    [HttpPost("{id}/stream")]
    [AccessTokenGuard(Role.BrandManager, Role.ShopManager)]
    public async Task<Uri> StreamCamera([FromRoute] Guid id)
    {
        return await streamingService.StreamCamera(id);
    }
}
