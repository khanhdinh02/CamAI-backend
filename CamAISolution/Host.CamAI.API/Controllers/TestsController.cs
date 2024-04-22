using Core.Application.Events;
using Core.Application.Events.Args;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Infrastructure.Observer.Messages;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Encodings;
using RabbitMQ.Client;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestsController(
    IBaseMapping mapping,
    IIncidentService incidentService,
    EventManager eventManager,
    IncidentSubject incidentSubject,
    ILogger<TestsController> logger,
    IReadFileService readFileService
) : ControllerBase
{
    [HttpPost("readfile")]
    public ActionResult ReadFile(IFormFile file)
    {
        // var path = "/home/ryuuji/coding/capstone/documents/test.csv";
        using var stream = new MemoryStream();
        file.CopyTo(stream);
        stream.Seek(0, SeekOrigin.Begin);
        foreach (var record in readFileService.ReadFile<EmployeeFromFileFormat>(stream, FileType.Csv))
            logger.LogInformation($"{record.Name}, {record.Email}");
        return Ok();
    }


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
}
