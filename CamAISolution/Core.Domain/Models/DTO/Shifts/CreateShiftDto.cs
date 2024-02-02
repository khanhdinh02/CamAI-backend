using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTO;

public class CreateShiftDto
{
    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }
}
