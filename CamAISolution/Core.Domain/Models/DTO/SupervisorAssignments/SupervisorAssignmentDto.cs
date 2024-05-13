using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class SupervisorAssignmentDto
{
    public Guid Id { get; set; }
    public Guid? ShopId { get; set; }
    public Guid? HeadSupervisorId { get; set; }
    public Guid? InChargeId { get; set; }
    public Role InChargeRole { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public AccountDto? HeadSupervisor { get; set; }
    public AccountDto? InCharge { get; set; }
    public List<IncidentDto> Incidents { get; set; } = [];
}
