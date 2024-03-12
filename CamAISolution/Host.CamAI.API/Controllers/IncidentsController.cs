using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IncidentsController(IBaseMapping mapping, IIncidentService incidentService) : ControllerBase
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
    [AccessTokenGuard(Role.ShopManager, Role.BrandManager)]
    public async Task<IncidentDto> GetIncidentById(Guid id)
    {
        return mapping.Map<Incident, IncidentDto>(await incidentService.GetIncidentById(id));
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
    [AccessTokenGuard(Role.ShopManager, Role.BrandManager)]
    public async Task<PaginationResult<IncidentDto>> GetIncident([FromQuery] SearchIncidentRequest searchRequest)
    {
        return mapping.Map<Incident, IncidentDto>(await incidentService.GetIncidents(searchRequest));
    }

    /// <summary>
    /// For shop manager to assign incident to employee
    /// It will also change the status of incident to Accepted
    /// </summary>
    /// <param name="id"></param>
    /// <param name="employeeId"></param>
    [HttpPut("{id}/employee/{employeeId}")]
    [AccessTokenGuard(Role.ShopManager)]
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
    [AccessTokenGuard(Role.ShopManager)]
    public async Task RejectIncident([FromRoute] Guid id)
    {
        await incidentService.RejectIncident(id);
    }
}
