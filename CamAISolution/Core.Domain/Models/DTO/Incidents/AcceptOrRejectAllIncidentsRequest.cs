namespace Core.Domain.DTO;

public record AcceptOrRejectAllIncidentsRequest(List<Guid> IncidentIds, Guid EmployeeId, bool IsAccept);
