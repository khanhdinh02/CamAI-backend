namespace Core.Domain.DTO;

public class EdgeBoxDto : BaseDto
{
    public string? Model { get; set; }
    public string? Version { get; set; }
    public BaseStatusDto EdgeBoxStatus { get; set; } = null!;
    public BaseStatusDto EdgeBoxLocation { get; set; } = null!;
}
