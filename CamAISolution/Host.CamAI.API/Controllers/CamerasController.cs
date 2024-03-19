using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CamerasController(ICameraService cameraService, IBaseMapping mapping) : ControllerBase
{
    [HttpGet("/api/shops/{shopId}/cameras")]
    public async Task<PaginationResult<CameraDto>> GetCameras([FromQuery] Guid shopId)
    {
        return mapping.Map<Camera, CameraDto>(await cameraService.GetCameras(shopId));
    }

    [HttpGet("{id}")]
    public async Task<CameraDto> GetCameraById([FromQuery] Guid id)
    {
        return mapping.Map<Camera, CameraDto>(await cameraService.GetCameraById(id));
    }
}
