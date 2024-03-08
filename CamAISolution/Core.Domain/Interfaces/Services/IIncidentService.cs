using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Services;

public interface IIncidentService
{
    Task<Incident> UpsertIncident(CreateIncidentDto incidentDto);
}