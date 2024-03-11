using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Interfaces.Services;

public interface IIncidentService
{
    Task<Incident> UpsertIncident(CreateIncidentDto incidentDto);
    Task<Incident> GetIncidentById(Guid id);
    Task<PaginationResult<Incident>> GetIncidents(SearchIncidentRequest searchRequest);

    Task AssignIncidentToEmployee(Guid id, Guid employeeId);
    Task RejectIncident(Guid id);
}
