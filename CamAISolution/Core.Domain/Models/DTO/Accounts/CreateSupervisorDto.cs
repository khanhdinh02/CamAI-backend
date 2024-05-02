using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTO;

public class CreateSupervisorDto
{
    public Guid EmployeeId { get; set; }

    [EmailAddress]
    public string Email { get; set; } = null!;
    public bool IsHeadSupervisor { get; set; }
}
