using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTOs;

public class CreateBrandDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    // TODO: email validation
    public string? Email { get; set; }

    // TODO: phone digit and length validation
    [StringLength(50)]
    public string? Phone { get; set; }
}
