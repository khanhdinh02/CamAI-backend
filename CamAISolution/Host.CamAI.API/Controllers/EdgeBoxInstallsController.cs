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
public class EdgeBoxInstallsController(IEdgeBoxInstallService edgeBoxInstallService, IBaseMapping mapper)
    : ControllerBase
{
    /// <summary>
    /// Get all installs, that has been activated and not disabled by admin, of a shop.
    /// </summary>
    /// <remarks>Use for Brand Manager and Shop Manager.</remarks>
    /// <param name="shopId"></param>
    /// <returns></returns>
    [HttpGet("/api/shops/{shopId}/installs")]
    [AccessTokenGuard(Role.Admin, Role.BrandManager, Role.ShopManager)]
    public async Task<IEnumerable<EdgeBoxInstallDto>> GetEdgeBoxInstallsByShop(Guid shopId)
    {
        var edgeBoxInstalls = await edgeBoxInstallService.GetInstallingByShop(shopId);
        return edgeBoxInstalls.Select(mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>);
    }

    /// <summary>
    /// Get all installs, that has been activated and not disabled by admin, of a brand.
    /// </summary>
    /// <remarks>Use for Brand Manager.</remarks>
    /// <param name="brandId"></param>
    /// <returns></returns>
    [HttpGet("/api/brands/{brandId}/installs")]
    [AccessTokenGuard(Role.Admin, Role.BrandManager)]
    public async Task<IEnumerable<EdgeBoxInstallDto>> GetEdgeBoxInstallsByBrand(Guid brandId)
    {
        var edgeBoxInstalls = await edgeBoxInstallService.GetInstallingByBrand(brandId);
        return edgeBoxInstalls.Select(mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>);
    }

    /// <summary>
    /// For admin: Get all install of a edge box
    /// </summary>
    /// <remarks>Use for Brand Manager.</remarks>
    /// <param name="edgeBoxId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("/api/edgeBoxes/{edgeBoxId}/installs")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<PaginationResult<EdgeBoxInstallDto>> GetEdgeBoxInstallsByEdgeBox(
        [FromRoute] Guid edgeBoxId,
        [FromQuery] SearchEdgeBoxInstallRequest request
    )
    {
        request.EdgeBoxId = edgeBoxId;
        var edgeBoxInstalls = await edgeBoxInstallService.GetEdgeBoxInstall(request);
        return mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>(edgeBoxInstalls);
    }

    /// <summary>
    /// Admin leases an edge box to a shop
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [AccessTokenGuard(Role.Admin)]
    public async Task<EdgeBoxInstallDto> LeaseEdgeBox(CreateEdgeBoxInstallDto dto)
    {
        var ebInstall = await edgeBoxInstallService.LeaseEdgeBox(dto);
        return mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>(ebInstall);
    }

    /// <summary>
    /// Brand manager activates an edge box
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut]
    [AccessTokenGuard(Role.BrandManager)]
    public async Task<EdgeBoxInstallDto> ActivateEdgeBox(ActivateEdgeBoxDto dto)
    {
        var ebInstall = await edgeBoxInstallService.ActivateEdgeBox(dto);
        return mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>(ebInstall);
    }
}
