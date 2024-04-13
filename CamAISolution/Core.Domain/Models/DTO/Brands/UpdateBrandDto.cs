using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTO;

public class UpdateBrandDto
{
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [EmailAddress]
    public string? Email { get; set; }

    [Phone]
    public string? Phone { get; set; }
    public string? Description { get; set; }
    public string CompanyName { get; set; } = null!;
    public string? BrandWebsite { get; set; }
    public string CompanyAddress { get; set; } = null!;
    public Guid CompanyWardId { get; set; }
}
