using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Services;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EdgeBoxesController(
    IEdgeBoxService edgeBoxService,
    IEdgeBoxInstallService edgeBoxInstallService,
    IBaseMapping mapping
) : ControllerBase
{
    /// <summary>
    /// </summary>
    /// <remarks>
    ///     Use for Admin.<br />
    ///     <c>BrandId</c>: Get all edge boxes that are currently installed for a brand.<br />
    ///     <c>ShopId</c>: Get all edge boxes that are currently installed for a shop.
    /// </remarks>
    /// <param name="searchRequest"></param>
    /// <returns></returns>
    [HttpGet]
    [AccessTokenGuard(Role.Admin)]
    public async Task<ActionResult<PaginationResult<EdgeBoxDto>>> GetEdgeBoxes(
        [FromQuery] SearchEdgeBoxRequest searchRequest
    )
    {
        var edgeBoxes = await edgeBoxService.GetEdgeBoxes(searchRequest);
        return Ok(mapping.Map<EdgeBox, EdgeBoxDto>(edgeBoxes));
    }

    [HttpGet("{id}")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<ActionResult<EdgeBoxDto>> GetEdgeBoxById([FromRoute] Guid id)
    {
        var edgeBox = await edgeBoxService.GetEdgeBoxById(id);
        return Ok(mapping.Map<EdgeBox, EdgeBoxDto>(edgeBox));
    }

    [HttpGet("{edgeBoxId}/activities")]
    [AccessTokenGuard(Role.Admin, Role.BrandManager, Role.ShopManager)]
    public async Task<PaginationResult<EdgeBoxInstallActivityDto>> GetEdgeBoxActivity(
        [FromRoute] Guid edgeBoxId,
        [FromQuery] EdgeBoxActivityByEdgeBoxIdSearchRequest searchRequest
    )
    {
        searchRequest.EdgeBoxId = edgeBoxId;
        var result = await edgeBoxInstallService.GetCurrentEdgeBoxInstallActivities(searchRequest);
        return mapping.Map<EdgeBoxInstallActivity, EdgeBoxInstallActivityDto>(result);
    }

    [HttpPost]
    [AccessTokenGuard(Role.Admin)]
    public async Task<ActionResult<EdgeBoxDto>> CreateEdgeBox([FromBody] CreateEdgeBoxDto edgeBoxDto)
    {
        var createdEdgeBox = await edgeBoxService.CreateEdgeBox(edgeBoxDto);
        return Ok(mapping.Map<EdgeBox, EdgeBoxDto>(createdEdgeBox));
    }

    [HttpPut("{id}")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<ActionResult<EdgeBoxDto>> UpdateEdgeBox(
        [FromRoute] Guid id,
        [FromBody] UpdateEdgeBoxDto edgeBoxDto
    )
    {
        var updatedEdgeBox = await edgeBoxService.UpdateEdgeBox(id, edgeBoxDto);
        return Ok(mapping.Map<EdgeBox, EdgeBoxDto>(updatedEdgeBox));
    }

    /// <summary>
    /// Allow admin to update the status of edge box (hardware)
    /// Active -> Broken: allow for all
    /// Active -> Inactive/Disposed, Broken -> Disposed: the latest eb install must be disabled
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    [HttpPut("{id}/status")]
    [AccessTokenGuard(Role.Admin)]
    public async Task UpdateEdgeBoxStatus([FromRoute] Guid id, [FromBody] UpdateEdgeBoxStatusDto dto)
    {
        await edgeBoxService.UpdateStatus(id, dto.Status);
    }

    /// <summary>
    /// Allow admin to update location of edge box
    /// Only 2 cases are allowed
    /// 1. Installing -> Occupied
    /// 2. Uninstalling -> Idle
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    [HttpPut("{id}/location")]
    [AccessTokenGuard(Role.Admin)]
    public async Task UpdateEdgeBoxLocation([FromRoute] Guid id, [FromBody] UpdateEdgeBoxLocationDto dto)
    {
        await edgeBoxService.UpdateEdgeBoxLocation(id, dto.Location);
    }

    [HttpDelete("{id}")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<IActionResult> DeleteEdgeBox([FromRoute] Guid id)
    {
        await edgeBoxService.DeleteEdgeBox(id);
        return Accepted();
    }
}
