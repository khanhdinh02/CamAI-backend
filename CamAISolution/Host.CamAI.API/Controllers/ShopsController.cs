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
public class ShopsController(IShopService shopService, IBaseMapping baseMapping) : ControllerBase
{
    /// <summary>
    /// Search Shop base on current account's roles.
    /// </summary>
    /// <param name="search"></param>
    /// <remarks>
    /// <para>
    ///     If current roles include Admin, admin can search every fields.
    /// </para>
    /// <para>
    ///     If current roles include Brand manager, brand manager can search every fields but BrandId will be set to current brand manager Id no matter what is your input.
    /// </para>
    /// <para>
    ///     If current roles include shop manager, shop manager can search every fields but ShopManagerId will bet set to current shop manager Id no matter what is your input
    /// </para>
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.BrandManager, RoleEnum.ShopManager)]
    public async Task<ActionResult<PaginationResult<ShopDto>>> GetCurrentShop([FromQuery] SearchShopRequest search)
    {
        var shops = await shopService.GetCurrentAccountShops(search);
        return Ok(baseMapping.Map<Shop, ShopDto>(shops));
    }

    [HttpPost]
    [AccessTokenGuard(RoleEnum.Admin)]
    public async Task<ActionResult<ShopDto>> CreateShop(CreateOrUpdateShopDto shopDto)
    {
        return Ok(baseMapping.Map<Shop, ShopDto>(await shopService.CreateShop(shopDto)));
    }

    [HttpPut("{id:guid}")]
    [AccessTokenGuard(RoleEnum.ShopManager, RoleEnum.Admin, RoleEnum.BrandManager)]
    public async Task<ActionResult<ShopDto>> UpdateShop(Guid id, [FromBody] CreateOrUpdateShopDto shopDto)
    {
        var updatedShop = await shopService.UpdateShop(id, shopDto);
        return Ok(baseMapping.Map<Shop, ShopDto>(updatedShop));
    }

    [HttpPatch("{id:guid}/status")]
    [AccessTokenGuard(RoleEnum.ShopManager, RoleEnum.Admin, RoleEnum.BrandManager)]
    public async Task<ActionResult<ShopDto>> UpdateShopStatus(Guid id, int shopStatusId)
    {
        var updatedShop = await shopService.UpdateStatus(id, shopStatusId);
        if (updatedShop.ShopStatusId == ShopStatusEnum.Inactive)
            return Ok();
        return Ok(baseMapping.Map<Shop, ShopDto>(updatedShop));
    }

    [HttpDelete("{id:guid}")]
    [AccessTokenGuard(RoleEnum.Admin)]
    public async Task<IActionResult> DeleteShop(Guid id)
    {
        await shopService.DeleteShop(id);
        return Accepted();
    }
}
