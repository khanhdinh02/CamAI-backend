using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTO;

public class CreateEdgeBoxDto
{
    [StringLength(50)]
    public string? Name { get; set; }
    public Guid EdgeBoxModelId { get; set; }
}
