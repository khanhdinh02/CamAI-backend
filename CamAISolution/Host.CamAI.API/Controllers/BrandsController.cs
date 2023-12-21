using AutoMapper;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandsController(IBrandService brandService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<PaginationResult<BrandDto>> GetBrands([FromQuery] SearchBrandRequest searchRequest)
    {
        var brands = await brandService.GetBrands(searchRequest);
        return mapper.MapPaginationResult<BrandDto, Brand>(brands);
    }

    [HttpGet("{id}")]
    public async Task<BrandDto> GetBrandById([FromRoute] Guid id)
    {
        var brand = await brandService.GetBrandById(id);
        return mapper.Map<BrandDto>(brand);
    }

    [HttpPost]
    public async Task<Brand> CreateBrand([FromBody] CreateBrandDto brandDto)
    {
        var brand = mapper.Map<Brand>(brandDto);
        return await brandService.CreateBrand(brand);
    }

    [HttpPut("{id}")]
    public async Task<Brand> UpdateBrand([FromRoute] Guid id, [FromBody] UpdateBrandDto brandDto)
    {
        return await brandService.UpdateBrand(id, brandDto);
    }

    [HttpPut("{id}/reactivate")]
    public async Task<Brand> ReactivateBrand([FromRoute] Guid id)
    {
        return await brandService.ReactivateBrand(id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrand([FromRoute] Guid id)
    {
        // TODO [Duy]: discuss what to return for delete
        await brandService.DeleteBrand(id);
        return NoContent();
    }

    // TODO [Duy]: endpoint to update logo
    // TODO [Duy]: endpoint to update banner
}
