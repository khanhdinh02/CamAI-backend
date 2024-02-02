using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTO;

public class ShiftDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid ShopId { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }
}
