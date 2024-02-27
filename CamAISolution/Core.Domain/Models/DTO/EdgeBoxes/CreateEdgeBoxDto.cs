using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTO;

public class CreateEdgeBoxDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string? Name { get; set; }
    public Guid EdgeBoxModelId { get; set; }
}
