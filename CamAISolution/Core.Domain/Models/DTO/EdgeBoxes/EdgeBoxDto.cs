using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class EdgeBoxDto : BaseDto
{
    public string? Model { get; set; }
    public string? Version { get; set; }
    public EdgeBoxStatus EdgeBoxStatus { get; set; }
    public EdgeBoxLocation EdgeBoxLocation { get; set; }
}
