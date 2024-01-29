using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTO;

public class CreateBrandDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(50)]
    [Phone]
    public string? Phone { get; set; }

    public CreateImageDto? Logo { get; set; }
    public CreateImageDto? Banner { get; set; }
}
