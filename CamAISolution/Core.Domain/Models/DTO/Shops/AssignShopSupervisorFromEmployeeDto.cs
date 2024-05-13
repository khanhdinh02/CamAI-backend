using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class AssignShopSupervisorFromEmployeeDto
{
    public Guid EmployeeId { get; set; }
    public Role Role { get; set; }
}
