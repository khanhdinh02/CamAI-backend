using System.ComponentModel.DataAnnotations;
using static Core.Domain.Entities.Account;

namespace Core.Domain;

public class ShopDto : BaseDto
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;
    [MaxLength(50)]
    public string? Phone { get; set; }
    public Guid WardId { get; set; }
    public string? AddressLine { get; set; }
    public ShopStatusDto Status { get; set; } = null!;
}
