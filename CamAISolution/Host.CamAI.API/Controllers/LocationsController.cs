using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationsController(ILocationService locationService, IBaseMapping mapper) : ControllerBase
{
    [HttpGet($"{nameof(Province)}s")]
    public async Task<IEnumerable<ProvinceDto>> GetAllProvinces()
    {
        return (await locationService.GetAllProvinces()).Select(mapper.Map<Province, ProvinceDto>);
    }

    /// <summary>
    /// Get Districts by Province's Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet($"{nameof(Province)}s/{{id}}/{nameof(District)}s")]
    public async Task<IEnumerable<DistrictDto>> GetAllDistrictsByProvinceId(int id)
    {
        return (await locationService.GetAllDistrictsByProvinceId(id)).Select(mapper.Map<District, DistrictDto>);
    }

    /// <summary>
    /// Get Wards by District's Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet($"{nameof(District)}s/{{id}}/{nameof(Ward)}s")]
    public async Task<IEnumerable<WardDto>> GetAllWardsByDistrictId(int id)
    {
        return (await locationService.GetAllWardsByDistrictId(id)).Select(mapper.Map<Ward, WardDto>);
    }
}
