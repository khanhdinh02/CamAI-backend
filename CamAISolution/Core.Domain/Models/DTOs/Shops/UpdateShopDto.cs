using System.ComponentModel.DataAnnotations;

namespace Core.Domain;

public class UpdateShopDto
{
    [MaxLength(50)]
    public string? Name { get; set; }

    [Phone, MaxLength(50)]
    public string? Phone { get; set; }
    public Guid? WardId { get; set; }
    public string? AddressLine { get; set; }
    public string? Status { get; set; }
}
