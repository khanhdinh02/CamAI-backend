using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Services;

public interface IEdgeBoxInstallService
{
    Task<EdgeBoxInstall> LeaseEdgeBox(CreateEdgeBoxInstallDto dto);
    Task<EdgeBoxInstall> ActivateEdgeBox(ActivateEdgeBoxDto dto);
}
