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
        return mapper.Map<IEnumerable<Province>, IEnumerable<ProvinceDto>>(await locationService.GetAllProvinces());
    }

    /// <summary>
    /// Get Districts by Province's Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet($"{nameof(Province)}s/{{id}}/{nameof(District)}s")]
    public async Task<IEnumerable<DistrictDto>> GetAllDistrictsByProvinceId(int id)
    {
        return mapper.Map<IEnumerable<District>, IEnumerable<DistrictDto>>(
            await locationService.GetAllDistrictsByProvinceId(id)
        );
    }

    /// <summary>
    /// Get Wards by District's Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet($"{nameof(District)}s/{{id}}/{nameof(Ward)}s")]
    public async Task<IEnumerable<WardDto>> GetAllWardsByDistrictId(int id)
    {
        return mapper.Map<IEnumerable<Ward>, IEnumerable<WardDto>>(await locationService.GetAllWardsByDistrictId(id));
    }
}
