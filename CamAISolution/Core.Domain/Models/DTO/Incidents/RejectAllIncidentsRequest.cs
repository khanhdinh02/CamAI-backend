namespace Core.Domain.DTO;

public record RejectAllIncidentsRequest(List<Guid> IncidentIds);
