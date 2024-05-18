using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Services;
using Infrastructure.Jwt.Attribute;
using Infrastructure.Mapping.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SupervisorAssignmentsController(
    ISupervisorAssignmentService supervisorAssignmentService,
    IEmployeeService employeeService,
    IIncidentService incidentService,
    IAccountService accountService,
    IBaseMapping mapping
) : Controller
{
    [HttpGet]
    [AccessTokenGuard(Role.ShopManager, Role.ShopHeadSupervisor, Role.ShopSupervisor)]
    public async Task<List<SupervisorAssignmentDto>> GetCalendar(DateTime date)
    {
        var assignments = await supervisorAssignmentService.GetSupervisorAssignmentByDate(date);
        FillEmptyAssignmentWithShopManager(assignments);
        var incidents = await GetIncidentsInDate(date);
        var assignmentDtos = assignments
            .Select(async assignment =>
                await SupervisorAssignmentProfile.ToSupervisorAssignmentDto(
                    mapping,
                    accountService,
                    employeeService,
                    assignment
                )
            )
            .Select(x => x.Result)
            .ToList();
        foreach (var dto in assignmentDtos)
        {
            var endTime = dto.EndTime ?? dto.StartTime.Date.AddDays(1).AddTicks(-1);
            dto.Incidents = incidents
                .Where(x =>
                    dto.StartTime <= x.StartTime
                    && x.StartTime <= endTime
                    && x.IncidentType is IncidentType.Phone or IncidentType.Uniform
                )
                .Select(mapping.Map<Incident, IncidentDto>)
                .ToList();
            dto.Interactions = incidents
                .Where(x =>
                    dto.StartTime <= x.StartTime && x.StartTime <= endTime && x.IncidentType == IncidentType.Interaction
                )
                .Select(mapping.Map<Incident, IncidentDto>)
                .ToList();
        }

        return assignmentDtos;
    }

    private void FillEmptyAssignmentWithShopManager(IList<SupervisorAssignment> assignments)
    {
        var account = accountService.GetCurrentAccount();
        if (account is not { Role: Role.ShopManager, ManagingShop: not null })
            return;

        var shop = account.ManagingShop!;
        if (TimeOnly.FromDateTime(DateTime.Now) <= shop.OpenTime)
            return;

        var i = 0;
        var assignmentCount = assignments.Count;
        if (assignmentCount == 0 || TimeOnly.FromDateTime(assignments[0].StartTime) > shop.OpenTime)
        {
            assignments.Insert(
                0,
                new SupervisorAssignment
                {
                    StartTime = DateTime.Today.Add(shop.OpenTime.ToTimeSpan()),
                    EndTime = assignments.Count != 0 ? assignments[0].StartTime : null
                }
            );
            i = 1;
        }

        while (i < assignments.Count - 1)
        {
            if (assignments[i].EndTime != assignments[i + 1].StartTime)
            {
                assignments.Insert(
                    i + 1,
                    new SupervisorAssignment
                    {
                        StartTime = assignments[i].EndTime!.Value,
                        EndTime = assignments[i + 1].StartTime
                    }
                );
            }

            i += 1;
        }

        if (
            assignmentCount != 0
            && assignments[^1].EndTime != null
            && TimeOnly.FromDateTime(assignments[^1].EndTime!.Value) < shop.CloseTime
        )
        {
            assignments.Insert(
                0,
                new SupervisorAssignment
                {
                    StartTime = DateTime.Today.Add(shop.OpenTime.ToTimeSpan()),
                    EndTime =
                        TimeOnly.FromDateTime(DateTime.Now) < shop.CloseTime
                            ? null
                            : DateTime.Today.Add(shop.CloseTime.ToTimeSpan())
                }
            );
        }
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

    [HttpGet("{assignmentId}/Incidents")]
    [AccessTokenGuard(Role.ShopManager, Role.BrandManager, Role.ShopHeadSupervisor, Role.ShopSupervisor)]
    public async Task<List<IncidentDto>> GetIncidentByAssignment(Guid assignmentId)
    {
        return (await incidentService.GetIncidentsByAssignment(assignmentId))
            .Select(mapping.Map<Incident, IncidentDto>)
            .ToList();
    }
}
