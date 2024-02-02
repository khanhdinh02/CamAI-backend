using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.Domain.DTO;

public class EmployeeShiftDto
{
    [Required]
    public Guid EmployeeId { get; set; }

    [Required]
    public Guid ShiftId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Required]
    public ShortDayOfWeek DayOfWeek { get; set; }

    [Required]
    public ShiftDto Shift { get; set; } = null!;
}
