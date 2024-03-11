using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Interfaces.Services;

public interface IIncidentService
{
    Task<Incident> UpsertIncident(CreateIncidentDto incidentDto);
    Task<Incident> GetIncidentById(Guid id);
    Task<PaginationResult<Incident>> GetIncidents(IncidentSearchRequest searchRequest);

    // TODO [Duy]: chagne the signature of these method
    // Task<Incident> AssignIncident(Guid id);
    // Task<Incident> UpdateStatus(Guid id);
}
