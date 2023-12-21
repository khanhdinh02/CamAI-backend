using AutoMapper;
using Core.Domain.DTOs;
using Core.Domain.Entities;
using Core.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShopsController(IShopService shopService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetShop([FromQuery] SearchShopRequest search)
    {
        var shops = await shopService.GetShops(search);
        return Ok(mapper.MapPaginationResult<ShopDto, Shop>(shops));
    }

    [HttpPost]
    public async Task<IActionResult> AddShop(CreateShopDto shopDto)
    {
        var createdShop = await shopService.CreateShop(mapper.Map<Shop>(shopDto));
        return Ok(createdShop);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateShop(Guid id, [FromBody] UpdateShopDto shopDto)
    {
        var updatedShop = await shopService.UpdateShop(id, shopDto);
        return Ok(mapper.Map<ShopDto>(updatedShop));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteShop(Guid id)
    {
        await shopService.DeleteShop(id);
        return Accepted();
    }
}
