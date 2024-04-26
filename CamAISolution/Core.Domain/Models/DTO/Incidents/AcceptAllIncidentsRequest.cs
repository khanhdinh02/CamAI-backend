namespace Core.Domain.DTO;

public record AcceptAllIncidentsRequest(List<Guid> IncidentIds, Guid EmployeeId);
