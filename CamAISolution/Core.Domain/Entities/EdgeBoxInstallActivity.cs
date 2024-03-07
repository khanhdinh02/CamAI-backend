using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;
public class EdgeBoxInstallActivity : ActivityEntity
{
    public Guid EdgeBoxInstallId { get; set; }
    public EdgeBoxInstallStatus OldStatus { get; set; }
    public EdgeBoxInstallStatus NewStatus { get; set; }

    public virtual EdgeBoxInstall EdgeBoxInstall { get; set; } = null!;
}
