using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandsController(IBrandService brandService, IBaseMapping mapping) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBrands([FromQuery] SearchBrandRequest searchRequest)
    {
        var brands = await brandService.GetBrands(searchRequest);
        return Ok(mapping.Map<Brand, BrandDto>(brands));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBrandById([FromRoute] Guid id)
    {
        var brand = await brandService.GetBrandById(id);
        return Ok(mapping.Map<Brand, BrandDto>(brand));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrand([FromBody] CreateBrandDto brandDto)
    {
        var createdBrand = await brandService.CreateBrand(mapping.Map<CreateBrandDto, Brand>(brandDto));
        return Ok(mapping.Map<Brand, BrandDto>(createdBrand));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBrand([FromRoute] Guid id, [FromBody] UpdateBrandDto brandDto)
    {
        var updatedBrand = await brandService.UpdateBrand(id, brandDto);
        return Ok(mapping.Map<Brand, BrandDto>(updatedBrand));
    }

    [HttpPut("{id}/reactivate")]
    public async Task<IActionResult> ReactivateBrand([FromRoute] Guid id)
    {
        var reactivatedBrand = await brandService.ReactivateBrand(id);
        return Ok(mapping.Map<Brand, BrandDto>(reactivatedBrand));
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
