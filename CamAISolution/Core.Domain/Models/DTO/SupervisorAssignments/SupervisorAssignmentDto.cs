namespace Core.Domain.DTO;

public class SupervisorAssignmentDto
{
    public Guid Id { get; set; }
    public Guid? ShopId { get; set; }
    public Guid? HeadSupervisorId { get; set; }
    public Guid? SupervisorId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public AccountDto? HeadSupervisor { get; set; }
    public AccountDto? Supervisor { get; set; }
    public List<IncidentDto> Incidents { get; set; } = [];
}
