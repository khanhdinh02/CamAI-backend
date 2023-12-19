using System.ComponentModel.DataAnnotations;

namespace Core.Domain;

public class CreateBrandDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }
}
