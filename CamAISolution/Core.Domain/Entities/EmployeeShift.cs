using System.ComponentModel.DataAnnotations;
using Core.Domain.DTO;

namespace Core.Domain.Entities;

public class EmployeeShift
{
    public Guid EmployeeId { get; set; }
    public Guid ShiftId { get; set; }

    [StringLength(3)]
    public ShortDayOfWeek DayOfWeek { get; set; }

    public virtual Employee Employee { get; set; } = null!;
    public virtual Shift Shift { get; set; } = null!;
}
