using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.Domain.DTO;

public class EmployeeShiftDto
{
    public Guid EmployeeId { get; set; }
    public Guid ShiftId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ShortDayOfWeek DayOfWeek { get; set; }
    public ShiftDto Shift { get; set; } = null!;
}
