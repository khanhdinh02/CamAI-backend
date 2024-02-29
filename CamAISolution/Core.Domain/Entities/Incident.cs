using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Incident : BusinessEntity
{
    public IncidentType IncidentType { get; set; }
    public DateTime Time { get; set; }

    public virtual IEnumerable<Employee> Employees { get; set; } = new HashSet<Employee>();
    public virtual ICollection<Evidence> Evidences { get; set; } = new HashSet<Evidence>();
}
