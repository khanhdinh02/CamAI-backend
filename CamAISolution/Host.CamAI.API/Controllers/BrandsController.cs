using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Models;
using Core.Domain.Services;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandsController(IBrandService brandService, IBaseMapping mapping) : ControllerBase
{
    [HttpGet]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.BrandManager, RoleEnum.ShopManager)]
    public async Task<ActionResult<PaginationResult<BrandDto>>> GetBrands([FromQuery] SearchBrandRequest searchRequest)
    {
        var brands = await brandService.GetBrands(searchRequest);
        return Ok(mapping.Map<Brand, BrandDto>(brands));
    }

    [HttpGet("{id}")]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.BrandManager, RoleEnum.ShopManager)]
    public async Task<ActionResult<BrandDto>> GetBrandById([FromRoute] Guid id)
    {
        var brand = await brandService.GetBrandById(id);
        return Ok(mapping.Map<Brand, BrandDto>(brand));
    }

    [HttpPost]
    [AccessTokenGuard(RoleEnum.Admin)]
    public async Task<ActionResult<BrandDto>> CreateBrand([FromBody] CreateBrandDto brandDto)
    {
        var createdBrand = await brandService.CreateBrand(mapping.Map<CreateBrandDto, Brand>(brandDto));
        return Ok(mapping.Map<Brand, BrandDto>(createdBrand));
    }

    [HttpPut("{id}")]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.BrandManager)]
    public async Task<ActionResult<BrandDto>> UpdateBrand([FromRoute] Guid id, [FromBody] UpdateBrandDto brandDto)
    {
        var updatedBrand = await brandService.UpdateBrand(id, brandDto);
        return Ok(mapping.Map<Brand, BrandDto>(updatedBrand));
    }

    [HttpPut("{id}/reactivate")]
    [AccessTokenGuard(RoleEnum.Admin)]
    public async Task<ActionResult<BrandDto>> ReactivateBrand([FromRoute] Guid id)
    {
        var reactivatedBrand = await brandService.ReactivateBrand(id);
        return Ok(mapping.Map<Brand, BrandDto>(reactivatedBrand));
    }

    [HttpDelete("{id}")]
    [AccessTokenGuard(RoleEnum.Admin)]
    public async Task<IActionResult> DeleteBrand([FromRoute] Guid id)
    {
        // TODO [Duy]: discuss what to return for delete
        await brandService.DeleteBrand(id);
        return Accepted();
    }

    // TODO [Duy]: endpoint to update logo
    // TODO [Duy]: endpoint to update banner
}
