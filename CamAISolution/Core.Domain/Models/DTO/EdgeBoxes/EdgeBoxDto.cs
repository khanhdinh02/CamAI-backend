namespace Core.Domain.DTO;

public class EdgeBoxDto : BaseDto
{
    public string? Model { get; set; }
    public string? Version { get; set; }
    public LookupDto EdgeBoxStatus { get; set; } = null!;
    public LookupDto EdgeBoxLocation { get; set; } = null!;
}
