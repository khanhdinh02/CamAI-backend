using AutoMapper;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers.Admins;
[Route("api/[controller]")]
[ApiController]
public class ShopsController(IShopService shopService, IMapper mapper) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetShop([FromQuery] SearchShopRequest search)
    {
        var shops = await shopService.GetShops(search);
        return Ok(new PaginationResult<ShopDto>
        {
            PageIndex = shops.PageIndex,
            PageSize = shops.PageSize,
            TotalCount = shops.TotalCount,
            Values = shops.Values.Select(s => mapper.Map<ShopDto>(s)).ToList()
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddShop(CreateShopDto shop)
    {
        var createdShop = await shopService.CreateShop(mapper.Map<Shop>(shop));
        return Ok(createdShop);
    }
}
