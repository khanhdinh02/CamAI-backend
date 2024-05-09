using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Services;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SupervisorAssignmentsController(
    ISupervisorAssignmentService supervisorAssignmentService,
    IIncidentService incidentService,
    IBaseMapping mapping
) : Controller
{
    [HttpGet]
    [AccessTokenGuard(Role.ShopManager, Role.ShopHeadSupervisor)]
    public async Task<List<SupervisorAssignmentDto>> GetCalendar(DateTime date)
    {
        var assignments = await supervisorAssignmentService.GetSupervisorAssignmentByDate(date);
        var incidents = await GetIncidentsInDate(date);
        var assignmentDtos = assignments.Select(mapping.Map<SupervisorAssignment, SupervisorAssignmentDto>).ToList();
        foreach (var dto in assignmentDtos)
        {
            var endTime = dto.EndTime ?? dto.StartTime.Date.AddDays(1).AddTicks(-1);
            dto.Incidents = incidents
                .Where(x => dto.StartTime <= x.StartTime && x.StartTime <= endTime)
                .Select(mapping.Map<Incident, IncidentDto>)
                .ToList();
        }

        return assignmentDtos;
    }

    private async Task<IList<Incident>> GetIncidentsInDate(DateTime date)
    {
        var startTime = date.Date;
        var endTime = startTime.AddDays(1).AddTicks(-1);
        var incidents = await incidentService.GetIncidents(
            new SearchIncidentRequest { FromTime = startTime, ToTime = endTime }
        );
        return incidents.Values;
    }
}
