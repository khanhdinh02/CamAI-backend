using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class SupervisorAssignmentDto
{
    public Guid Id { get; set; }
    public Guid? ShopId { get; set; }
    public Guid? SupervisorId { get; set; }
    public Guid? InChargeAccountId { get; set; }
    public Role InChargeAccountRole { get; set; }
    public Guid? InChargeEmployeeId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public AccountDto? Supervisor { get; set; }
    public AccountDto? InChargeAccount { get; set; }
    public List<IncidentDto> Incidents { get; set; } = [];
    public List<IncidentDto> Interactions { get; set; } = [];
}
