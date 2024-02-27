namespace Core.Domain.DTO;

public class EdgeBoxModelDto : BaseDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ModelCode { get; set; }
    public string? Manufacturer { get; set; }
    public string? CPU { get; set; }
    public string? RAM { get; set; }
    public string? Storage { get; set; }
    public string? OS { get; set; }
}
