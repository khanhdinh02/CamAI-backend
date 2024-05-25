using System.Net.WebSockets;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Host.CamAI.API.Sockets;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IncidentsController(
    IBaseMapping mapping,
    IIncidentService incidentService,
    IncidentSocketManager incidentSocketManager,
    ILogger<IncidentsController> logger
) : ControllerBase
{
    /// <summary>
    /// For shop manager and brand manager
    /// </summary>
    /// <remarks>
    /// <para>
    ///     Shop manager can only get incidents inside their shop
    /// </para>
    /// <para>
    ///     Brand manager can get incidents inside their brand
    /// </para>
    /// </remarks>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [AccessTokenGuard(Role.ShopManager, Role.BrandManager, Role.ShopSupervisor)]
    public async Task<IncidentDto> GetIncidentById(Guid id)
    {
        return mapping.Map<Incident, IncidentDto>(await incidentService.GetIncidentById(id, true));
    }

    /// <summary>
    /// For shop manager and brand manager
    /// </summary>
    /// <remarks>
    /// <para>
    ///     Shop manager can only get incidents inside their shop
    /// </para>
    /// <para>
    ///     Brand manager can get incidents inside their brand
    /// </para>
    /// </remarks>
    /// <param name="searchRequest"></param>
    /// <returns></returns>
    [HttpGet]
    [AccessTokenGuard(Role.ShopManager, Role.BrandManager, Role.ShopSupervisor)]
    public async Task<PaginationResult<IncidentDto>> GetIncident([FromQuery] SearchIncidentRequest searchRequest)
    {
        return mapping.Map<Incident, IncidentDto>(await incidentService.GetIncidents(searchRequest));
    }

    [HttpPost("accept")]
    [AccessTokenGuard(Role.ShopManager, Role.ShopSupervisor)]
    public async Task<IActionResult> AcceptAllIncidents(AcceptAllIncidentsRequest request)
    {
        await incidentService.AcceptOrRejectAllIncidents(request.IncidentIds, request.EmployeeId, true);
        return Ok();
    }

    [HttpPost("reject")]
    [AccessTokenGuard(Role.ShopManager, Role.ShopSupervisor)]
    public async Task<IActionResult> RejectAllIncidents(RejectAllIncidentsRequest request)
    {
        await incidentService.AcceptOrRejectAllIncidents(request.IncidentIds, Guid.Empty, false);
        return Ok();
    }

    /// <summary>
    /// For shop manager to assign incident to employee
    /// It will also change the status of incident to Accepted
    /// </summary>
    /// <param name="id"></param>
    /// <param name="employeeId"></param>
    [HttpPut("{id}/employee/{employeeId}")]
    [AccessTokenGuard(Role.ShopManager, Role.ShopSupervisor)]
    public async Task AssignIncidentToEmployee([FromRoute] Guid id, [FromRoute] Guid employeeId)
    {
        await incidentService.AssignIncidentToEmployee(id, employeeId);
    }

    /// <summary>
    /// For shop manager to reject an incident
    /// It will change the status of incident to Rejected
    /// And remove link to employee if previously assigned
    /// </summary>
    /// <param name="id"></param>
    [HttpPut("{id}/reject")]
    [AccessTokenGuard(Role.ShopManager, Role.ShopSupervisor)]
    public async Task RejectIncident([FromRoute] Guid id)
    {
        await incidentService.RejectIncident(id);
    }

    [HttpGet("new")]
    [AccessTokenGuard(Role.ShopManager)]
    public async Task GetNewestIncident()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest is false)
            return;
        var account = (Account)HttpContext.Items[nameof(Account)]!;
        var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        try
        {
            incidentSocketManager.AddSocket(account.Id, socket);
            await socket.SendAsync(
                System.Text.Encoding.UTF8.GetBytes("Connected"),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
            var buffer = new byte[1024 * 4];
            var result = await socket.ReceiveAsync(
                new ArraySegment<byte>(buffer, 0, buffer.Length),
                CancellationToken.None
            );

            // wait until have close message
            while (result.MessageType != WebSocketMessageType.Close)
            {
                result = await socket.ReceiveAsync(
                    new ArraySegment<byte>(buffer, 0, buffer.Length),
                    CancellationToken.None
                );
            }
        }
        catch (Exception ex) when (ex is WebSocketException)
        {
            logger.LogWarning("Websocket exception occurred");
            logger.LogError(ex, ex.Message);
        }
        finally
        {
            incidentSocketManager.RemoveSocket(account.Id, socket);
        }
    }

    /// <summary>
    /// For Manager to get the count of incidents for a shop.
    /// Shop manager does not need to specify shopId.
    /// </summary>
    /// <param name="shopId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="interval"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    [HttpGet("count")]
    [AccessTokenGuard(Role.ShopManager, Role.BrandManager)]
    public async Task<IncidentCountDto> CountIncidentsByShop(
        Guid? shopId,
        DateOnly startDate,
        DateOnly endDate,
        ReportInterval interval,
        IncidentTypeRequestOption type
    )
    {
        return await incidentService.CountIncidentsByShop(shopId, startDate, endDate, interval, type);
    }

    [HttpGet("percent")]
    [AccessTokenGuard(Role.ShopManager, Role.BrandManager)]
    public async Task<IncidentPercentDto> GetIncidentPercent(Guid? shopId, DateOnly startDate, DateOnly endDate)
    {
        return await incidentService.GetIncidentPercent(shopId, startDate, endDate);
    }
}
