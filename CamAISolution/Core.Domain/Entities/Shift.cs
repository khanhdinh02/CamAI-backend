using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Shift : BusinessEntity
{
    public Guid ShopId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public virtual Shop Shop { get; set; } = null!;
    public virtual ICollection<EmployeeShift> EmployeeShifts { get; set; } = new HashSet<EmployeeShift>();
}
