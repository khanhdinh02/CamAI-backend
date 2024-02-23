using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Incident : BusinessEntity
{
    public IncidentType IncidentType { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? CameraId { get; set; }
    public DateTime Time { get; set; }

    public virtual Employee? Employee { get; set; }
    public virtual Camera? Camera { get; set; }
    public virtual ICollection<Evidence> Evidences { get; set; } = new HashSet<Evidence>();
}
