using Core.Application.Events;
using Core.Application.Events.Args;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;
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
    IUnitOfWork unitOfWork
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
        cacheService.Set(
            "test",
            "test",
            TimeSpan.FromSeconds(1),
            (key, value) =>
            {
                logger.LogInformation($"{key} and {value}");
                cacheService.Set(keyChange, $"{key} changed at {DateTimeHelper.VNDateTime}", TimeSpan.FromDays(1));
            }
        );
        return Ok();
    }

    [HttpGet("cache-value")]
    public IActionResult TestGetCache()
    {
        return Ok(cacheService.Get<string>("change"));
    }

    [HttpGet("noti-incident/")]
    public async Task<IActionResult> NotificationIncident([FromQuery] Role sentTo)
    {
        var result = (await unitOfWork.Accounts.GetAsync(expression: a => a.Role == sentTo, takeAll: true)).Values;
        foreach (var value in result)
        {
            incidentSubject.Notify(
                new CreatedOrUpdatedIncidentArgs(
                    new()
                    {
                        CreatedDate = DateTimeHelper.VNDateTime,
                        IncidentType = IncidentType.Uniform,
                        Status = IncidentStatus.New,
                    },
                    IncidentEventType.NewIncident,
                    value.Id
                )
            );
        }

        return Ok();
    }

    [HttpGet("clean-image")]
    public async Task<IActionResult> CleanImages()
    {
        if (HttpContext.Connection.RemoteIpAddress.Equals(HttpContext.Connection.LocalIpAddress))
        {
            logger.LogInformation("Local address, Refuse delete");
            return Ok("Local address, Refuse delete");
        }
        var recordsCount = 0;
        foreach (
            var evidence in (
                await unitOfWork.Evidences.GetAsync(takeAll: true, includeProperties: [nameof(Evidence.Image)])
            ).Values.Where(e => e.Image != null)
        )
        {
            if (!System.IO.File.Exists(evidence.Image!.PhysicalPath))
            {
                recordsCount++;
                logger.LogWarning(
                    "Remove empty image {ImageId} and evidence {EvidenceId}",
                    evidence.Image.Id,
                    evidence.Id
                );
                unitOfWork.GetRepository<Image>().Delete(evidence.Image);
                unitOfWork.Evidences.Delete(evidence);
            }
        }
        await unitOfWork.CompleteAsync();
        logger.LogWarning("{NumberOfRecords} images and evidences have been deteled", recordsCount);
        return Ok();
    }

    [HttpGet("clean-incident")]
    public async Task<IActionResult> ClearIncients()
    {
        var incidents = await unitOfWork.Incidents.GetAsync(expression: i => i.Evidences.Any(), takeAll: true);
        foreach (var incident in incidents.Values)
        {
            unitOfWork.Incidents.Delete(incident);
        }
        logger.LogInformation("{NumberOfRecords} incidents have been deleted", incidents.Values.Count);
        return Ok();
    }
}
