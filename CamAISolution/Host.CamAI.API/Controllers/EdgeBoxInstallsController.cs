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
