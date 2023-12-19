using AutoMapper;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandsController(IBrandService brandService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<PaginationResult<BrandDto>> GetBrands()
    {
        // TODO pagingResult util extensions
        var pagingResult = await brandService.GetBrands();
        return new PaginationResult<BrandDto>();
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
        var resultBrand = await brandService.CreateBrand(brand);
        return resultBrand;
    }

    [HttpPut("{id}")]
    public async Task<Brand> UpdateBrand([FromRoute] Guid id, [FromBody] UpdateBrandDto brandDto)
    {
        var brand = mapper.Map<Brand>(brandDto);
        brand.Id = id;
        var resultBrand = await brandService.UpdateBrand(brand);
        return resultBrand;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrand([FromRoute] Guid id)
    {
        await brandService.DeleteBrand(id);
        return NoContent();
    }
}
