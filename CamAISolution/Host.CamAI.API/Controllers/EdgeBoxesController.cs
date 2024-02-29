using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Models;
using Core.Domain.Services;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EdgeBoxesController(IEdgeBoxService edgeBoxService, IBaseMapping mapping) : ControllerBase
{
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

    [HttpDelete("{id}")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<IActionResult> DeleteEdgeBox([FromRoute] Guid id)
    {
        // TODO [Duy]: discuss what to return for delete
        await edgeBoxService.DeleteEdgeBox(id);
        return Accepted();
    }
}
