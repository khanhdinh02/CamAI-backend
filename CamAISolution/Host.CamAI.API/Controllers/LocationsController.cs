using Core.Domain.Entities;
using Core.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//TODO [Dat] : Map to DTO and cast type for ActionResult
public class LocationsController(ILocationService locationService) : ControllerBase
{
    [HttpGet($"{nameof(Province)}s")]
    public async Task<ActionResult> GetAllProvinces()
    {
        return Ok(await locationService.GetAllProvinces());
    }

    /// <summary>
    /// Get Districts by Province's Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet($"{nameof(Province)}s/{{id}}/{nameof(District)}s")]
    public async Task<ActionResult> GetAllDistrictsByProvinceId(int id)
    {
        var districts = await locationService.GetAllDistrictsByProvinceId(id);
        return Ok(
            districts.Select(d =>
            {
                d.Province = null;
                return d;
            })
        );
    }

    /// <summary>
    /// Get Wards by District's Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet($"{nameof(District)}s/{{id}}/{nameof(Ward)}s")]
    public async Task<ActionResult> GetAllWardsByDistrictId(int id)
    {
        var wards = await locationService.GetAllWardsByDistrictId(id);
        return Ok(
            wards.Select(w =>
            {
                w.District = null;
                return w;
            })
        );
    }
}
