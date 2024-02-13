namespace Core.Domain.DTO;

public class ShiftDto
{
    public Guid Id { get; set; }
    public Guid ShopId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
