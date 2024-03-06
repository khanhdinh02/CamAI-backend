using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EdgeBoxModelsController(IEdgeBoxModelService edgeBoxModelService, IBaseMapping mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<EdgeBoxModelDto>> GetAll()
    {
        return (await edgeBoxModelService.GetAll()).Select(mapper.Map<EdgeBoxModel, EdgeBoxModelDto>);
    }
}
