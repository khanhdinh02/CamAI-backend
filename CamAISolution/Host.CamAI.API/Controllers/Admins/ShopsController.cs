using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShopsController(IShopService shopService, IBaseMapping baseMapping) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetShop([FromQuery] SearchShopRequest search)
    {
        var shops = await shopService.GetShops(search);
        return Ok(baseMapping.Map<Shop, ShopDto>(shops));
    }

    [HttpPost]
    public async Task<IActionResult> CreateShop(CreateOrUpdateShopDto shopDto)
    {
        var createdShop = await shopService.CreateShop(baseMapping.Map<CreateOrUpdateShopDto, Shop>(shopDto));
        return Ok(createdShop);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateShop(Guid id, [FromBody] CreateOrUpdateShopDto shopDto)
    {
        var updatedShop = await shopService.UpdateShop(id, shopDto);
        return Ok(baseMapping.Map<Shop, ShopDto>(updatedShop));
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateShopStatus(Guid id, Guid shopStatusId)
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
