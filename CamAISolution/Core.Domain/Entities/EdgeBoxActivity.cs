using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class EdgeBoxActivity : ActivityEntity
{
    public Guid? EdgeBoxId { get; set; }
    public Guid? EdgeBoxInstallId { get; set; }
    public EdgeBoxActivityType Type { get; set; }

    public virtual EdgeBox? EdgeBox { get; set; }
    public virtual EdgeBoxInstall? EdgeBoxInstall { get; set; }
}
