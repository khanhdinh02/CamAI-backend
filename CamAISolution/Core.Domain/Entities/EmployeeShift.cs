using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities;

public class EmployeeShift
{
    public Guid EmployeeId { get; set; }
    public Guid ShiftId { get; set; }

    [StringLength(9)]
    public DayOfWeek DayOfWeek { get; set; }

    public virtual Employee Employee { get; set; } = null!;
    public virtual Shift Shift { get; set; } = null!;
}
