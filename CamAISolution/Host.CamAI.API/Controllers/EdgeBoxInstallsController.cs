using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
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
    [AccessTokenGuard(Role.BrandManager, Role.ShopManager)]
    public async Task<IEnumerable<EdgeBoxInstallDto>> GetEdgeBoxInstallsByShop(Guid shopId)
    {
        var edgeBoxInstalls = await edgeBoxInstallService.GetInstallingByShop(shopId);
        return edgeBoxInstalls.Select(mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>);
    }

    [HttpPost]
    [AccessTokenGuard(Role.Admin)]
    public async Task<EdgeBoxInstallDto> LeaseEdgeBox(CreateEdgeBoxInstallDto dto)
    {
        var ebInstall = await edgeBoxInstallService.LeaseEdgeBox(dto);
        return mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>(ebInstall);
    }

    [HttpPut]
    [AccessTokenGuard(Role.BrandManager)]
    public async Task<EdgeBoxInstallDto> ActivateEdgeBox(ActivateEdgeBoxDto dto)
    {
        var ebInstall = await edgeBoxInstallService.ActivateEdgeBox(dto);
        return mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>(ebInstall);
    }
}
