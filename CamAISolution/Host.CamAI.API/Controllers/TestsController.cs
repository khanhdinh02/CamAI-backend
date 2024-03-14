using Core.Application.Events;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Services;
using Infrastructure.Observer.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Timer = System.Timers.Timer;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestsController(
    IBaseMapping mapping,
    IIncidentService incidentService,
    IEdgeBoxService edgeBoxService,
    IEdgeBoxInstallService edgeBoxInstallService,
    EventManager eventManager,
    IPublishEndpoint bus)
    : ControllerBase
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

    [HttpPatch("activate/edge-box/{edgeBoxId:guid}")]
    public async Task<ActionResult> ActivateEdgeBox(Guid edgeBoxId)
    {
        // eventManager.NotifyActivatedEdgeBox(edgeBoxId);
        var activatedEdgeBoxMessage = new ActivatedEdgeBoxMessage
        {
            RoutingKey = edgeBoxId.ToString("N"),
            Message = "Hello"
        };

        // Create Timer object for handling edge box status after time interval.
        // TODO[Dat]: Timer or Task.Delay ?
        SetTimer(edgeBoxId, 5000);

        await edgeBoxService.UpdateStatus(edgeBoxId, EdgeBoxStatus.Activating);
        await bus.Publish(activatedEdgeBoxMessage);

        return Ok();
    }

    private void SetTimer(Guid edgeBoxId, long interval)
    {
        var timer = new Timer(interval);
        timer.Elapsed += async (_, _) => await Handle(edgeBoxId);
        timer.AutoReset = false;
        timer.Start();
    }

    private async Task Handle(Guid edgeBoxId)
    {
        var edgeBox = await edgeBoxService.GetEdgeBoxById(edgeBoxId);

        // This is indicating that edge box is still disconnected from server
        if (edgeBox.EdgeBoxStatus == EdgeBoxStatus.Activating)
        {
            await edgeBoxService.UpdateStatus(edgeBoxId, EdgeBoxStatus.Inactive);
        }
    }
}