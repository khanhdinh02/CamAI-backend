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
    public string? Description { get; set; }
    public string CompanyName { get; set; } = null!;
    public string? BrandWebsite { get; set; }
    public string CompanyAddress { get; set; } = null!;
    public int CompanyWardId { get; set; }
}
