using System.Text;
using Core.Application.Events;
using Core.Application.Events.Args;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Services;
using Core.Domain.Utilities;
using Infrastructure.Observer.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestsController(
    IBaseMapping mapping,
    IIncidentService incidentService,
    EventManager eventManager,
    IncidentSubject incidentSubject,
    ILogger<TestsController> logger,
    ICacheService cacheService,
) : ControllerBase
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
            Phone = message.Phone
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
            Phone = message.Phone
        };
        eventManager.NotifyShopChanged(shop);
    }

    [HttpPost("incident")]
    public async Task<IncidentDto> UpsertIncident([FromBody] CreateIncidentDto dto)
    {
        var incident = await incidentService.UpsertIncident(dto);
        return mapping.Map<Incident, IncidentDto>(incident);
    }

    [HttpGet("test-incident")]
    public ActionResult TestRealTimeIncident()
    {
        incidentSubject.Notify(
            new CreatedOrUpdatedIncidentArgs(
                new Incident(),
                IncidentEventType.NewIncident,
                Guid.Parse("8A6E9252-E300-4546-9BDB-E01144266B0F")
            )
        );
        return Ok();
    }

    [HttpGet("cache")]
    public ActionResult TestAddCache()
    {
        var keyChange = "change";
        cacheService.Set("test", "test", TimeSpan.FromSeconds(1), (key, value) =>
        {
            logger.LogInformation($"{key} and {value}");
            cacheService.Set(keyChange, $"{key} changed at {DateTimeHelper.VNDateTime}", TimeSpan.FromDays(1));
        });
        return Ok();
    }

    [HttpGet("cache-value")]
    public IActionResult TestGetCache()
    {
        return Ok(cacheService.Get<string>("change"));
    }

    [HttpGet("noti-incident")]
    public IActionResult NotificationIncident(Guid sentTo)
    {
        incidentSubject.Notify(new CreatedOrUpdatedIncidentArgs(new()
        {
            CreatedDate = DateTimeHelper.VNDateTime,
            IncidentType = Core.Domain.Enums.IncidentType.Uniform,
            Status = Core.Domain.Enums.IncidentStatus.New,
        }, IncidentEventType.NewIncident, sentTo));
        return Ok();
    }
}
