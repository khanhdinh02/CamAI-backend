using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTO.Accounts;
using Core.Domain.Models.DTO.Shops;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers.Admins;

[Route("api/[controller]")]
[ApiController]
public class ShopsController(IShopService shopService, IBaseMapping baseMapping) : ControllerBase
{
    [HttpGet]
    [AccessTokenGuard]
    public async Task<IActionResult> GetShop([FromQuery] SearchShopRequest search)
    {
        var shops = await shopService.GetShops(search);
        return Ok(baseMapping.Map<Shop, ShopDto>(shops));
    }

    [HttpPost]
    [AccessTokenGuard(RoleEnum.Admin)]
    public async Task<IActionResult> CreateShop(CreateOrUpdateShopDto shopDto)
    {
        return Ok(baseMapping.Map<Shop, ShopDto>(await shopService.CreateShop(shopDto)));
    }

    [HttpPut("{id:guid}")]
    [AccessTokenGuard(RoleEnum.ShopManager, RoleEnum.Admin, RoleEnum.BrandManager)]
    public async Task<IActionResult> UpdateShop(Guid id, [FromBody] CreateOrUpdateShopDto shopDto)
    {
        var updatedShop = await shopService.UpdateShop(id, shopDto);
        return Ok(baseMapping.Map<Shop, ShopDto>(updatedShop));
    }

    [HttpPatch("{id:guid}")]
    [AccessTokenGuard(RoleEnum.ShopManager, RoleEnum.Admin, RoleEnum.BrandManager)]
    public async Task<IActionResult> UpdateShopStatus(Guid id, int shopStatusId)
    {
        Shop updatedShop = await shopService.UpdateStatus(id, shopStatusId);
        if (updatedShop.ShopStatusId == ShopStatusEnum.Inactive)
            return Ok();
        return Ok(baseMapping.Map<Shop, ShopDto>(updatedShop));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteShop(Guid id)
    {
        await shopService.DeleteShop(id);
        return Accepted();
    }
}
