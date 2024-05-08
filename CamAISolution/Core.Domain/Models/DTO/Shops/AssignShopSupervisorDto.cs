using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class AssignShopSupervisorDto
{
    public Guid AccountId { get; set; }
    public Role Role { get; set; }
}
