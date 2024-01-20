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
    /// <summary>
    /// Search brand base on current account's roles.
    /// </summary>
    /// <param name="searchRequest"></param>
    /// <remarks>
    /// <para>
    ///     Admin can search every brands.
    /// </para>
    /// <para>
    ///     Brand manager can only get their brand.
    /// </para>
    /// <para>
    ///     Shop manager can only get the brand that the shop is in.
    /// </para>
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.BrandManager, RoleEnum.ShopManager)]
    public async Task<ActionResult<PaginationResult<BrandDto>>> GetBrands([FromQuery] SearchBrandRequest searchRequest)
    {
        var brands = await brandService.GetBrands(searchRequest);
        return Ok(mapping.Map<Brand, BrandDto>(brands));
    }

    /// <summary>
    /// Get brand by id, the access will be depend on account's roles
    /// </summary>
    /// <param name="id"></param>
    /// <para>
    ///     Admin can get every brand.
    /// </para>
    /// <para>
    ///     Brand manager can get the brand they manage.
    /// </para>
    /// <para>
    ///     Shop manager can get the brand their shop is in.
    /// </para>
    /// <returns></returns>
    [HttpGet("{id}")]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.BrandManager, RoleEnum.ShopManager)]
    public async Task<ActionResult<BrandDto>> GetBrandById([FromRoute] Guid id)
    {
        var brand = await brandService.GetBrandById(id);
        return Ok(mapping.Map<Brand, BrandDto>(brand));
    }

    /// <summary>
    /// Create brand. Only Admin can do it.
    /// </summary>
    /// <param name="brandDto"></param>
    /// <returns></returns>
    [HttpPost]
    [AccessTokenGuard(RoleEnum.Admin)]
    public async Task<ActionResult<BrandDto>> CreateBrand([FromBody] CreateBrandDto brandDto)
    {
        var createdBrand = await brandService.CreateBrand(mapping.Map<CreateBrandDto, Brand>(brandDto));
        return Ok(mapping.Map<Brand, BrandDto>(createdBrand));
    }

    /// <summary>
    /// Update brand. Admin and brand manager can do it
    /// </summary>
    /// <param name="id"></param>
    /// <param name="brandDto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.BrandManager)]
    public async Task<ActionResult<BrandDto>> UpdateBrand([FromRoute] Guid id, [FromBody] UpdateBrandDto brandDto)
    {
        var updatedBrand = await brandService.UpdateBrand(id, brandDto);
        return Ok(mapping.Map<Brand, BrandDto>(updatedBrand));
    }

    /// <summary>
    /// Admin can reactivate the disabled brand
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id}/reactivate")]
    [AccessTokenGuard(RoleEnum.Admin)]
    public async Task<ActionResult<BrandDto>> ReactivateBrand([FromRoute] Guid id)
    {
        var reactivatedBrand = await brandService.ReactivateBrand(id);
        return Ok(mapping.Map<Brand, BrandDto>(reactivatedBrand));
    }

    /// <summary>
    /// Delete brand. Only Admin can do it.
    /// </summary>
    /// <param name="id"></param>
    /// <para>
    ///     If brand has related entity then the brand will be disabled.
    /// </para>
    /// <para>
    ///     If brand does not has related entity then the brand will be deleted entirely from the system
    /// </para>
    /// <returns></returns>
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
