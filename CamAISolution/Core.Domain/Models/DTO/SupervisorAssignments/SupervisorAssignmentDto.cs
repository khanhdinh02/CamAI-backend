using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class SupervisorAssignmentDto
{
    public Guid Id { get; set; }
    public Guid? ShopId { get; set; }
    public Guid? AssignorId { get; set; }
    public Guid? AssigneeId { get; set; }
    public Role AssignedRole { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}
