namespace Core.Domain.DTO;

public class CreateShiftDto
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
