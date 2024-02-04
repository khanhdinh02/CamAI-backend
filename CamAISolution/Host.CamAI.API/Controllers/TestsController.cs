using Core.Application.Events;
using Core.Domain.Entities;
using Infrastructure.Observer.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestsController(EventManager eventManager) : ControllerBase
{
    [HttpGet]
    public ActionResult<string> TestEndpoint()
    {
        return Ok(new { data = $"Hello From {nameof(TestsController)}" });
    }

    [HttpPost("sync/brand")]
    public void SyncData(BrandUpdateMessage message)
    {
        var brand = new Brand
        {
            Name = message.Name,
            Email = message.Email,
            Phone = message.Phone,
        };
        eventManager.NotifyBrandChanged(brand);
    }

    [HttpPost("sync/shop")]
    public void SyncData(ShopUpdateMessage message)
    {
        var shop = new Shop
        {
            Name = message.Name,
            AddressLine = message.Address,
            Phone = message.Phone,
        };
        eventManager.NotifyShopChanged(shop);
    }
}
