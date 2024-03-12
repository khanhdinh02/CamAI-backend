using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class EmployeeDto : BaseDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Gender Gender { get; set; }
    public string? Phone { get; set; }
    public Uri? Image { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? AddressLine { get; set; }
    public int? WardId { get; set; }
    public Guid? ShopId { get; set; }
    public EmployeeStatus EmployeeStatus { get; set; }

    public WardDto? Ward { get; set; }
    public ShopDto? Shop { get; set; }
}
